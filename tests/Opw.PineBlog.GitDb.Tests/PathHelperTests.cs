using FluentAssertions;
using Xunit;

namespace Opw.PineBlog.GitDb
{
    public class PathHelperTests
    {
        [Fact]
        public void Build_Should_ReturnPath()
        {
            var result = PathHelper.Build("A", "B", "C");

            result.Should().Be("A/B/C");
        }

        [Fact]
        public void Build_Should_TrimSlashes()
        {
            var result = PathHelper.Build("/A", "\\B", "C");

            result.Should().Be("A/B/C");
        }

        [Fact]
        public void Build_Should_IgnoreWhiteSpaceParts()
        {
            var result = PathHelper.Build("A", " ", "", null, "B", "C");

            result.Should().Be("A/B/C");
        }
    }
}
