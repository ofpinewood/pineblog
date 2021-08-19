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
    // TODO: add tests
    public class GitDbSyncService : IHostedService, IDisposable
    {
        private Timer _timer;
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

            _timer = new Timer(Sync, null, TimeSpan.Zero, TimeSpan.FromSeconds(_options.Value.SyncFrequency));

            return Task.CompletedTask;
        }

        private void Sync(object state)
        {
            var basePath = _options.Value.LocalRepositoryBasePath;
            if (!Directory.Exists(basePath))
                return;

            // Stop timer until sync finished;
            _timer?.Change(Timeout.Infinite, 0);

            try
            {
                using (var context = GitDbContext.CreateFromLocal(_options.Value))
                {
                    var branchesSynced = context.Sync();
                    if (branchesSynced < 1)
                        _logger.LogError($"Could not sync repository \"{_options.Value.RepositoryUrl}\".");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Could not sync repository \"{_options.Value.RepositoryUrl}\".");
            }

            _timer?.Change(TimeSpan.FromSeconds(_options.Value.SyncFrequency), TimeSpan.FromSeconds(_options.Value.SyncFrequency));

            _logger.LogInformation($"GitDbSyncService: \"{_options.Value.RepositoryUrl}\" synced.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("GitDbSyncService: Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
