using FluentAssertions;
using Xunit;

namespace Opw.PineBlog.Models
{
    public class PagerTests
    {
        [Fact]
        public void Configure_Should_ConfigureForPage1()
        {
            var pager = new Pager(1, 5);

            pager.Configure(15, "page={0}");

            pager.CurrentPage.Should().Be(1);
            pager.ItemsPerPage.Should().Be(5);
            pager.Total.Should().Be(15);
            pager.LastPage.Should().Be(3);
            pager.NotFound.Should().BeFalse();

            pager.Newer.Should().Be(0);
            pager.ShowNewer.Should().BeFalse();
            pager.LinkToNewer.Should().BeNull();

            pager.Older.Should().Be(2);
            pager.ShowOlder.Should().BeTrue();
            pager.LinkToOlder.Should().Be("page=2");
        }

        [Fact]
        public void Configure_Should_ConfigureForPage2()
        {
            var pager = new Pager(2, 5);

            pager.Configure(15, "page={0}");

            pager.CurrentPage.Should().Be(2);
            pager.ItemsPerPage.Should().Be(5);
            pager.Total.Should().Be(15);
            pager.LastPage.Should().Be(3);
            pager.NotFound.Should().BeFalse();

            pager.Newer.Should().Be(1);
            pager.ShowNewer.Should().BeTrue();
            pager.LinkToNewer.Should().Be("page=1");

            pager.Older.Should().Be(3);
            pager.ShowOlder.Should().BeTrue();
            pager.LinkToOlder.Should().Be("page=3");
        }

        [Fact]
        public void Configure_Should_ConfigureForPage3()
        {
            var pager = new Pager(3, 5);

            pager.Configure(15, "page={0}");

            pager.CurrentPage.Should().Be(3);
            pager.ItemsPerPage.Should().Be(5);
            pager.Total.Should().Be(15);
            pager.LastPage.Should().Be(3);
            pager.NotFound.Should().BeFalse();

            pager.Newer.Should().Be(2);
            pager.ShowNewer.Should().BeTrue();
            pager.LinkToNewer.Should().Be("page=2");

            pager.Older.Should().Be(4);
            pager.ShowOlder.Should().BeFalse();
            pager.LinkToOlder.Should().BeNull();
        }

        [Fact]
        public void Configure_Should_ConfigureForPage1_WithOnly1Page()
        {
            var pager = new Pager(1, 5);

            pager.Configure(4, "page={0}");

            pager.CurrentPage.Should().Be(1);
            pager.ItemsPerPage.Should().Be(5);
            pager.Total.Should().Be(4);
            pager.LastPage.Should().Be(1);
            pager.NotFound.Should().BeFalse();

            pager.Newer.Should().Be(0);
            pager.ShowNewer.Should().BeFalse();
            pager.LinkToNewer.Should().BeNull();

            pager.Older.Should().Be(2);
            pager.ShowOlder.Should().BeFalse();
            pager.LinkToOlder.Should().BeNull();
        }

        [Fact]
        public void Configure_Should_ConfigureForPage2_WithOnly2Pages()
        {
            var pager = new Pager(2, 5);

            pager.Configure(7, "page={0}");

            pager.CurrentPage.Should().Be(2);
            pager.ItemsPerPage.Should().Be(5);
            pager.Total.Should().Be(7);
            pager.LastPage.Should().Be(2);
            pager.NotFound.Should().BeFalse();

            pager.Newer.Should().Be(1);
            pager.ShowNewer.Should().BeTrue();
            pager.LinkToNewer.Should().Be("page=1");

            pager.Older.Should().Be(3);
            pager.ShowOlder.Should().BeFalse();
            pager.LinkToOlder.Should().BeNull();
        }
    }
}
