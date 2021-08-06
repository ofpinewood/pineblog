using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Opw.PineBlog.GitDb.LibGit2
{
    public class GitContextTests : GitTestsBase
    {
        private readonly GitContext _gitContext;

        public GitContextTests()
        {
            var options = ServiceProvider.GetService<IOptions<GitSettings>>();
            _gitContext = GitContext.Create(options.Value);
        }
    }
}
