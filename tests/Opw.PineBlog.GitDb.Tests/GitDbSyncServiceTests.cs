using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.GitDb
{
    public class GitDbSyncServiceTests : GitDbTestsBase
    {
        private readonly Mock<ILogger<GitDbSyncService>> _loggerMock;
        private readonly IOptions<PineBlogGitDbOptions> _options;
        private readonly TestGitDbSyncService _gitDbSyncService;

        public GitDbSyncServiceTests(GitDbFixture fixture) : base(fixture)
        {
            _options = ServiceProvider.GetRequiredService<IOptions<PineBlogGitDbOptions>>();
            _loggerMock = new Mock<ILogger<GitDbSyncService>>();

            _gitDbSyncService = new TestGitDbSyncService(_options, _loggerMock.Object);
        }

        [Fact]
        public async Task StartAsync_Should_LogInformationAndSetTimer()
        {
            await _gitDbSyncService.StartAsync(default);

            _gitDbSyncService.GetTimer().Should().NotBeNull();

            _loggerMock.Verify(
                m => m.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("GitDbSyncService: Hosted Service is starting.", StringComparison.OrdinalIgnoreCase)),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((_, __) => true)), Times.Exactly(1));
        }

        [Fact]
        public async Task Sync_Should_LogInformationAndSetTimer()
        {
            await _gitDbSyncService.StartAsync(default);

            for (int i = 0; i < 3; i++)
            {
                Thread.Sleep(500);
            }

            _loggerMock.Verify(
                m => m.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v.ToString().Contains($"GitDbSyncService: \"{_options.Value.RepositoryUrl}\" synced.", StringComparison.OrdinalIgnoreCase)),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((_, __) => true)), Times.Exactly(1));
        }

        [Fact]
        public async Task StopAsync_Should_LogInformation()
        {
            await _gitDbSyncService.StopAsync(default);

            _gitDbSyncService.GetTimer().Should().BeNull();

            _loggerMock.Verify(
                m => m.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("GitDbSyncService: Hosted Service is stopping.", StringComparison.OrdinalIgnoreCase)),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((_, __) => true)), Times.Exactly(1));
        }
    }
}
