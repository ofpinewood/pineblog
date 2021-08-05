using LibGit2Sharp;
using Opw.HttpExceptions;
using System;
using System.Collections.Generic;
using System.IO;

namespace Opw.PineBlog.Git.LibGit2
{
    public class GitContext : IDisposable
    {
        private readonly Repository _repository;
        private readonly UsernamePasswordCredentials _credentials;
        private readonly string _signatureName;
        private readonly string _signatureEmail;
        private readonly string _repositoryPath;

        private GitContext(Repository repository, UsernamePasswordCredentials credentials, string signatureName, string signatureEmail, string repositoryPath)
        {
            _repository = repository;
            _credentials = credentials;
            _signatureName = signatureName;
            _signatureEmail = signatureEmail;
            _repositoryPath = repositoryPath;
        }

        public static GitContext Create(GitSettings settings)
        {
            var credentials = new UsernamePasswordCredentials { Username = settings.UserName, Password = settings.Password };
            var path = Path.Combine(settings.LocalRepositoryBasePath, settings.Workdirpath);

            string repositoryPath = Repository.Discover(path);
            if (repositoryPath == null)
            {
                var cloneOptions = new CloneOptions();
                cloneOptions.CredentialsProvider = (_url, _user, _cred) => credentials;

                var sourceUrl = new Uri(settings.SourceUrl).AbsoluteUri;
                repositoryPath = Repository.Clone(sourceUrl, path, cloneOptions);
                if (repositoryPath == null)
                {
                    throw new NotFoundException<GitContext>($"Could not clone repository \"{sourceUrl}\".");
                }
            }

            var repository = new Repository(path);
            return new GitContext(repository, credentials, settings.UserName, settings.UserEmail, repositoryPath);
        }

        public static GitContext CreateFromLocal(GitSettings settings)
        {
            var credentials = new UsernamePasswordCredentials { Username = settings.UserName, Password = settings.Password };
            var path = Path.Combine(settings.LocalRepositoryBasePath, settings.Workdirpath);

            string repositoryPath = Repository.Discover(path);
            if (repositoryPath == null)
                return null;

            var repository = new Repository(path);
            return new GitContext(repository, credentials, settings.UserName, settings.UserEmail, repositoryPath);
        }

        public Result<IDictionary<string, byte[]>> GetFiles(string path)
        {
            var fileBytes = new Dictionary<string, byte[]>();

            path += "";
            var fullPath = Path.Combine(_repository.Info.WorkingDirectory, path);
            if (!Directory.Exists(fullPath))
                return Result<IDictionary<string, byte[]>>.Fail(new GitException($"Path not found \"{path}\"."));

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
                    return Result<IDictionary<string, byte[]>>.Fail(new GitException($"Could not read file \"{file}\".", ex));
                }
            }

            return Result<IDictionary<string, byte[]>>.Success(fileBytes);
        }

        public Result<IDictionary<string, byte[]>> GetFiles(IEnumerable<string> files)
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
                    return Result<IDictionary<string, byte[]>>.Fail(new GitException($"Could not read file \"{file}\".", ex));
                }
            }

            return Result<IDictionary<string, byte[]>>.Success(fileBytes);
        }

        IEnumerable<string> SearchDirectory(string path)
        {
            var files = new List<string>();
            foreach (string directory in Directory.GetDirectories(path))
            {
                if (directory.EndsWith(".git"))
                    continue;
                files.AddRange(SearchDirectory(directory));
            }

            files.AddRange(Directory.GetFiles(path));
            return files;
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}
