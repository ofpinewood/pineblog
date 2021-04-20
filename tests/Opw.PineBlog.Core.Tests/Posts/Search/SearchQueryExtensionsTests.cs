using FluentAssertions;
using Xunit;

namespace Opw.PineBlog.Posts.Search
{
    public class SearchQueryExtensionsTests
    {
        [Fact]
        public void ParseTerms_Should_ConvertMultipleSpaceIntoOne_ForQueryNull()
        {
            string query = null;

            var result = query.ParseTerms();

            result.Should().HaveCount(0);
        }

        [Fact]
        public void ParseTerms_Should_ConvertMultipleSpaceIntoOne_ForQuery()
        {
            var query = "c# dotnet  pineblog  ";

            var result = query.ParseTerms();

            result.Should().HaveCount(3);
            result.Should().BeEquivalentTo(new string[] { "c#", "dotnet", "pineblog" });
        }

        [Fact]
        public void ParseTerms_Should_ToLower_ForQuery()
        {
            var query = " C# DOTNET  pineblog  ";

            var result = query.ParseTerms();

            result.Should().HaveCount(3);
            result.Should().BeEquivalentTo(new string[] { "c#", "dotnet", "pineblog" });
        }

        [Fact]
        public void ParseTerms_Should_3Terms_ForQuery()
        {
            var query = " C# DOTNET  pineblog  ";

            var result = query.ParseTerms();

            result.Should().HaveCount(3);
            result.Should().BeEquivalentTo(new string[] { "c#", "dotnet", "pineblog" });
        }
    }
}
