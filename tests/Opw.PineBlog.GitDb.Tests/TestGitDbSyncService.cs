using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;

namespace Opw.PineBlog.GitDb
{
    public class TestGitDbSyncService : GitDbSyncService
    {
        public TestGitDbSyncService(
            IOptions<PineBlogGitDbOptions> options,
            ILogger<GitDbSyncService> logger)
            : base(options, logger)
        { }

        public Timer GetTimer()
        {
            return Timer;
        }
    }
}
