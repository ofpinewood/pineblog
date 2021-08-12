using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.GitDb.LibGit2
{
    public class GitDbContextTests : GitTestsBase
    {
        private readonly GitDbContext _gitDbContext;

        public GitDbContextTests()
        {
            var options = ServiceProvider.GetService<IOptions<PineBlogGitDbOptions>>();
            _gitDbContext = GitDbContext.Create(options.Value);
        }

        [Fact]
        public void GetFiles_PathRoot_3Files()
        {
            var result = _gitDbContext.GetFiles(string.Empty);

            result.IsSuccess.Should().BeTrue();

            var files = result.Value;
            files.Should().HaveCount(3);

            files.Should().ContainKey(".gitignore");
            files.Should().ContainKey("LICENSE");
            files.Should().ContainKey("README.md");

            files["LICENSE"].Should().HaveCount(1090);
        }

        [Fact]
        public void GetFiles_PathRootBranchTest_4Files()
        {
            var checkoutBranchResult = _gitDbContext.CheckoutBranch("test");
            checkoutBranchResult.IsSuccess.Should().BeTrue();

            var result = _gitDbContext.GetFiles(string.Empty);

            result.IsSuccess.Should().BeTrue();

            var files = result.Value;
            files.Should().HaveCount(4);

            files.Should().ContainKey(".gitignore");
            files.Should().ContainKey("LICENSE");
            files.Should().ContainKey("README.md");
            files.Should().ContainKey("Test.md");
            
            files["LICENSE"].Should().HaveCount(1090);
        }

        [Fact]
        public void GetFiles_2SpecificFiles_2Files()
        {
            var result = _gitDbContext.GetFiles(new string[] { "LICENSE", "README.md" });

            result.IsSuccess.Should().BeTrue();

            var files = result.Value;
            files.Should().HaveCount(2);

            files.Should().ContainKey("LICENSE");
            files.Should().ContainKey("README.md");

            files["LICENSE"].Should().HaveCount(1090);
        }
    }
}
