using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Opw.PineBlog.GitDb.LibGit2;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.GitDb
{
    public class GitDbSyncService : IHostedService, IDisposable
    {
        protected Timer Timer;
        private readonly IOptions<PineBlogGitDbOptions> _options;
        private readonly ILogger<GitDbSyncService> _logger;

        public GitDbSyncService(
            IOptions<PineBlogGitDbOptions> options,
            ILogger<GitDbSyncService> logger)
        {
            _options = options;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("GitDbSyncService: Hosted Service is starting.");

            Timer = new Timer(Sync, null, TimeSpan.Zero, TimeSpan.FromSeconds(_options.Value.SyncFrequency));

            return Task.CompletedTask;
        }

        private void Sync(object state)
        {
            var basePath = _options.Value.LocalRepositoryBasePath;
            if (!Directory.Exists(basePath))
                return;

            // Stop timer until sync finished;
            Timer?.Change(Timeout.Infinite, 0);

            try
            {
                using (var context = GitDbContext.CreateFromLocal(_options.Value))
                {
                    var branchesSynced = context.Sync();
                    if (branchesSynced < 1)
                        _logger.LogError($"Could not sync repository \"{_options.Value.RepositoryUrl}\".");

                    CheckBlogSettingsFileChanges(context);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Could not sync repository \"{_options.Value.RepositoryUrl}\".");
            }

            Timer?.Change(TimeSpan.FromSeconds(_options.Value.SyncFrequency), TimeSpan.FromSeconds(_options.Value.SyncFrequency));

            _logger.LogInformation($"GitDbSyncService: \"{_options.Value.RepositoryUrl}\" synced.");
        }

        private void CheckBlogSettingsFileChanges(GitDbContext context)
        {
            var path = PathHelper.Build(_options.Value.LocalRepositoryBasePath, _options.Value.RootPath, GitDbConstants.BlogSettingsFile);
            if (!File.Exists(path))
                return;

            var lastModifiedDate = File.GetLastWriteTimeUtc(path);
            if (lastModifiedDate != null && lastModifiedDate > DateTime.UtcNow.AddSeconds(-10))
            {
                path = PathHelper.Build(_options.Value.RootPath, GitDbConstants.BlogSettingsFile);
                var files = context.GetFiles(new string[] { path });
                FileChangeObserver.Instance.OnChanged(new FileChangeEventArgs(GitDbConstants.BlogSettingsFile, files[path]));
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("GitDbSyncService: Hosted Service is stopping.");

            Timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Timer?.Dispose();
        }
    }
}
