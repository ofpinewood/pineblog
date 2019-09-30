using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Opw.PineBlog.Entities;
using Opw.PineBlog.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.Posts
{
    public class GetPagedPostListQueryTests : MediatRTestsBase
    {
        public GetPagedPostListQueryTests() : base()
        {
            SeedDatabase();
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_With3Posts()
        {
            var result = await Mediator.Send(new GetPagedPostListQuery { Page = 1 });

            result.IsSuccess.Should().BeTrue();
            result.Value.Posts.Should().HaveCount(3);
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_With3Posts_WithItemsPerPage2()
        {
            var result = await Mediator.Send(new GetPagedPostListQuery { Page = 1, ItemsPerPage = 2 });

            result.IsSuccess.Should().BeTrue();
            result.Value.Posts.Should().HaveCount(2);
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_With2Posts()
        {
            var result = await Mediator.Send(new GetPagedPostListQuery { Page = 2 });

            result.IsSuccess.Should().BeTrue();
            result.Value.Posts.Should().HaveCount(2);
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_With6Posts_WhenIncludingUnpublishedPosts()
        {
            var result = await Mediator.Send(new GetPagedPostListQuery { Page = 1, IncludeUnpublished = true, ItemsPerPage = 100 });

            result.IsSuccess.Should().BeTrue();
            result.Value.Posts.Should().HaveCount(6);
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_WithBlogCoverUrlFormatReplaced()
        {
            var result = await Mediator.Send(new GetPagedPostListQuery());

            result.IsSuccess.Should().BeTrue();
            result.Value.Blog.CoverUrl.Should().Be("http://127.0.0.1:10000/devstoreaccount1/pineblog-tests/blog/cover-image.png");
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_WithPostListTypeBlog()
        {
            var result = await Mediator.Send(new GetPagedPostListQuery());

            result.IsSuccess.Should().BeTrue();
            result.Value.PostListType.Should().Be(PostListType.Blog);
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_WhenFilterOnCategory_WithPostListTypeCategory()
        {
            var result = await Mediator.Send(new GetPagedPostListQuery { Category = "category" });

            result.IsSuccess.Should().BeTrue();
            result.Value.PostListType.Should().Be(PostListType.Category);
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_WithBlogInfo()
        {
            var result = await Mediator.Send(new GetPagedPostListQuery());

            result.IsSuccess.Should().BeTrue();
            result.Value.Posts.Should().HaveCount(3);
            result.Value.Blog.Title.Should().Be("Title from configuration");
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_WithPostCovers()
        {
            var result = await Mediator.Send(new GetPagedPostListQuery());

            result.IsSuccess.Should().BeTrue();
            result.Value.Posts.Should().HaveCount(3);
            result.Value.Posts.First().CoverUrl.Should().Be("https://ofpinewood.com/cover-url");
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_WithPostAuthors()
        {
            var result = await Mediator.Send(new GetPagedPostListQuery());

            result.IsSuccess.Should().BeTrue();
            result.Value.Posts.Should().HaveCount(3);
            result.Value.Posts.First().Author.DisplayName.Should().Be("Author 1");
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_WithPager()
        {
            var result = await Mediator.Send(new GetPagedPostListQuery());

            result.IsSuccess.Should().BeTrue();
            result.Value.Pager.CurrentPage.Should().Be(1);
            result.Value.Pager.ItemsPerPage.Should().Be(3);
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_With3Posts_ForCategoryCat2()
        {
            var result = await Mediator.Send(new GetPagedPostListQuery { Page = 1, Category = "cat2", ItemsPerPage = 100 });

            result.IsSuccess.Should().BeTrue();
            result.Value.Posts.Should().HaveCount(3);
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_With1Post_ForCategoryCat3()
        {
            var result = await Mediator.Send(new GetPagedPostListQuery { Page = 1, Category = "cat3", ItemsPerPage = 100 });

            result.IsSuccess.Should().BeTrue();
            result.Value.Posts.Should().HaveCount(1);
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_With2Posts_ForCategoryCat3IncludingUnpublishedPosts()
        {
            var result = await Mediator.Send(new GetPagedPostListQuery { Page = 1, IncludeUnpublished = true, Category = "cat3", ItemsPerPage = 100 });

            result.IsSuccess.Should().BeTrue();
            result.Value.Posts.Should().HaveCount(2);
        }

        private void SeedDatabase()
        {
            var context = ServiceProvider.GetRequiredService<IBlogEntityDbContext>();

            var author = new Author { UserName = "user@example.com", DisplayName = "Author 1" };
            context.Authors.Add(author);
            context.SaveChanges();

            context.Posts.Add(CreatePost(0, author.Id, true, false, "cat1"));
            context.Posts.Add(CreatePost(1, author.Id, true, true, "cat1"));
            context.Posts.Add(CreatePost(2, author.Id, true, true, "cat1,cat2"));
            context.Posts.Add(CreatePost(3, author.Id, true, true, "cat2"));
            context.Posts.Add(CreatePost(4, author.Id, true, true, "cat1,cat2,cat3"));
            context.Posts.Add(CreatePost(5, author.Id, false, true, "cat3"));
            context.SaveChanges();
        }

        private Post CreatePost(int i, Guid authorId, bool published, bool cover, string categories)
        {
            var post = new Post
            {
                AuthorId = authorId,
                Title = "Post title " + i,
                Slug = "post-title-" + i,
                Categories = categories,
                Description = "Description",
                Content = "Content"
            };

            if (published) post.Published = DateTime.UtcNow;
            if (cover)
            {
                post.CoverUrl = "https://ofpinewood.com/cover-url";
                post.CoverCaption = "Cover caption";
                post.CoverLink = "https://ofpinewood.com/cover-link";
            }

            return post;
        }
    }
}
