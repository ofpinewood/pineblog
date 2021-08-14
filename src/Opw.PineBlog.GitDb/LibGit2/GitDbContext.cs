using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using Opw.HttpExceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.GitDb.LibGit2
{
    // TODO: make better async
    public class GitDbContext : IDisposable
    {
        private readonly Repository _repository;
        private readonly Credentials _credentials;
        private readonly string _repositoryPath;

        // if there are merge conflicts we always take the remote version
        private readonly MergeOptions _mergeOptionsTheirs = new MergeOptions()
        {
            FileConflictStrategy = CheckoutFileConflictStrategy.Theirs,
            MergeFileFavor = MergeFileFavor.Theirs,
            IgnoreWhitespaceChange = true,
            //CommitOnSuccess = true,
            //FailOnConflict = true
        };

        private readonly CheckoutOptions _checkoutOptionsForce = new CheckoutOptions()
        {
            CheckoutModifiers = CheckoutModifiers.Force,
            CheckoutNotifyFlags = CheckoutNotifyFlags.None,
        };

        private GitDbContext(Repository repository, Credentials credentials, string repositoryPath)
        {
            _repository = repository;
            _credentials = credentials;
            _repositoryPath = repositoryPath;
        }

        public static async Task<GitDbContext> CreateAsync(PineBlogGitDbOptions options, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await Task.Run(() => Create(options));
        }

        protected static GitDbContext Create(PineBlogGitDbOptions options)
        {
            var credentials = GetCredentials(options);

            string repositoryPath = Repository.Discover(options.LocalRepositoryBasePath);
            if (repositoryPath == null)
            {
                var cloneOptions = new CloneOptions();
                cloneOptions.CredentialsProvider = (_url, _user, _cred) => credentials;

                var sourceUrl = new Uri(options.RepositoryUrl).AbsoluteUri;
                repositoryPath = Repository.Clone(sourceUrl, options.LocalRepositoryBasePath, cloneOptions);
                if (repositoryPath == null)
                {
                    throw new NotFoundException<GitDbContext>($"Could not clone repository \"{sourceUrl}\".");
                }
            }

            var repository = new Repository(options.LocalRepositoryBasePath);
            return new GitDbContext(repository, credentials, repositoryPath);
        }

        public async Task<Branch> CheckoutBranchAsync(string branchName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await Task.Run(() => CheckoutBranch(branchName));
        }

        protected Branch CheckoutBranch(string branchName)
        {
            Branch branch = null;
            // if the branch is already checked out return it
            if (_repository.Head.FriendlyName == branchName)
            {
                branch = _repository.Head;
            }
            if (branch != null)
                return branch;

            // check if we have a local version to checkout
            branch = _repository.Branches.FirstOrDefault(b => b.FriendlyName == branchName);
            if (branch != null)
            {
                Commands.Checkout(_repository, branch, _checkoutOptionsForce);
                return branch;
            }

            // get the branch from the remote            
            // fetch the latest info from the remotes
            Fetch();

            // retry to get the remote branch
            branchName = $"origin/{branchName}";
            branch = _repository.Branches.FirstOrDefault(b => b.FriendlyName == branchName);
            if (branch == null)
                throw new NotFoundException<Branch>(branchName);

            // pull the remote branch
            branch = Pull(branch, _mergeOptionsTheirs);
            return branch;
        }

        public async Task<IDictionary<string, byte[]>> GetFilesAsync(string path, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await Task.Run(() => GetFiles(path));
        }

        protected IDictionary<string, byte[]> GetFiles(string path)
        {
            path += "";
            var fullPath = Path.Combine(_repository.Info.WorkingDirectory, path);
            if (!Directory.Exists(fullPath))
                throw new GitDbException($"Path not found \"{path}\".");

            var fileBytes = new Dictionary<string, byte[]>();
            var files = SearchDirectory(fullPath);
            foreach (var file in files)
            {
                try
                {
                    var bytes = File.ReadAllBytes(Path.Combine(_repository.Info.WorkingDirectory, file));
                    fileBytes.Add(file.Substring(_repository.Info.WorkingDirectory.Length), bytes);
                }
                catch (Exception ex)
                {
                    throw new GitDbException($"Could not read file \"{file}\".", ex);
                }
            }

            return fileBytes;
        }

        public async Task<IDictionary<string, byte[]>> GetFilesAsync(IEnumerable<string> files, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await Task.Run(() => GetFiles(files));
        }

        protected IDictionary<string, byte[]> GetFiles(IEnumerable<string> files)
        {
            var fileBytes = new Dictionary<string, byte[]>();
            foreach (var file in files)
            {
                try
                {
                    var bytes = File.ReadAllBytes(Path.Combine(_repository.Info.WorkingDirectory, file));
                    fileBytes.Add(file, bytes);
                }
                catch (Exception ex)
                {
                    throw new GitDbException($"Could not read file \"{file}\".", ex);
                }
            }

            return fileBytes;
        }

        protected static Credentials GetCredentials(PineBlogGitDbOptions options)
        {
            //return new UsernamePasswordCredentials { Username = settings.UserName, Password = settings.Password };
            return new DefaultCredentials();
        }

        protected IEnumerable<string> SearchDirectory(string path, bool recursive = false)
        {
            var files = new List<string>();

            if (recursive)
            {
                foreach (string directory in Directory.GetDirectories(path))
                {
                    if (directory.EndsWith(".git"))
                        continue;
                    files.AddRange(SearchDirectory(directory));
                }
            }

            files.AddRange(Directory.GetFiles(path));
            return files;
        }

        protected void Fetch()
        {
            string logMessage = null;
            var fetchOptions = new FetchOptions { CredentialsProvider = new CredentialsHandler((url, usernameFromUrl, types) => _credentials) };

            var remotes = _repository.Network.Remotes;
            foreach (var remote in remotes)
            {
                var refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);
                Commands.Fetch(_repository, remote.Name, refSpecs, fetchOptions, logMessage);
            }
        }

        protected Branch Pull(Branch branch, MergeOptions mergeOptions)
        {
            var signature = new Signature(name: "PineBlog GitDb", email: "pineblog-gitdb@ofpinewood.com", new DateTimeOffset(DateTime.UtcNow));
            var fetchOptions = new FetchOptions { CredentialsProvider = new CredentialsHandler((url, usernameFromUrl, types) => _credentials) };
            var pullOptions = new PullOptions();
            pullOptions.FetchOptions = fetchOptions;
            pullOptions.MergeOptions = mergeOptions;

            if (branch.IsRemote)
            {
                var localBranch = _repository.Branches.SingleOrDefault(b => !b.IsRemote && b.CanonicalName == branch.CanonicalName);
                if (localBranch == null)
                {
                    var branchName = branch.CanonicalName.Substring(branch.CanonicalName.IndexOf("origin/") + "origin/".Length);
                    localBranch = _repository.CreateBranch(branchName, branch.Tip);
                    _repository.Branches.Update(localBranch, b => b.TrackedBranch = branch.CanonicalName);
                    localBranch = _repository.Branches[branchName];
                }
                branch = localBranch;
            }

            // set active branch
            Commands.Checkout(_repository, branch);
            Commands.Pull(_repository, signature, pullOptions);

            return branch;
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}
