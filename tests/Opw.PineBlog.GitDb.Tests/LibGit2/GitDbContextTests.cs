using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.GitDb.LibGit2
{
    public class GitDbContextTests : GitDbTestsBase
    {
        [Fact]
        public async Task GetFilesAsync_PathRoot_3Files()
        {
            var gitDbContext = await GetGitDbContextAsync();

            var files = await gitDbContext.GetFilesAsync(string.Empty, CancellationToken.None);

            files.Should().HaveCount(3);

            files.Should().ContainKey(".gitignore");
            files.Should().ContainKey("LICENSE");
            files.Should().ContainKey("README.md");

            files["LICENSE"].Should().HaveCount(1090);
        }

        [Fact(Skip = "Checking out another branch while running the unit test will randomly fail other tests.")]
        public async Task GetFilesAsync_PathRootBranchTest_4Files()
        {
            var gitDbContext = await GetGitDbContextAsync();
            await gitDbContext.CheckoutBranchAsync("test", CancellationToken.None);

            var files = await gitDbContext.GetFilesAsync(string.Empty, CancellationToken.None);

            files.Should().HaveCount(4);

            files.Should().ContainKey(".gitignore");
            files.Should().ContainKey("LICENSE");
            files.Should().ContainKey("README.md");
            files.Should().ContainKey("Test.md");
            
            files["LICENSE"].Should().HaveCount(1090);
        }

        [Fact]
        public async Task GetFilesAsync_2SpecificFiles_2Files()
        {
            var gitDbContext = await GetGitDbContextAsync();

            var files = await gitDbContext.GetFilesAsync(new string[] { "LICENSE", "README.md" }, CancellationToken.None);

            files.Should().HaveCount(2);

            files.Should().ContainKey("LICENSE");
            files.Should().ContainKey("README.md");

            files["LICENSE"].Should().HaveCount(1090);
        }

        private async Task<GitDbContext> GetGitDbContextAsync()
        {
            var options = ServiceProvider.GetService<IOptions<PineBlogGitDbOptions>>();
            var gitDbContext = await GitDbContext.CreateAsync(options.Value, CancellationToken.None);

            await gitDbContext.CheckoutBranchAsync("main", CancellationToken.None);

            return gitDbContext;
        }
    }
}
