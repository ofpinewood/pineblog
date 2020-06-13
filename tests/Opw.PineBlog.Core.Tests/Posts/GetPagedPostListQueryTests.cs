using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Opw.PineBlog.Entities;
using Opw.PineBlog.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.Posts
{
    public class GetPagedPostListQueryTests : MediatRTestsBase
    {
        public GetPagedPostListQueryTests() : base()
        {
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_With3Posts()
        {
            await SeedDatabase();

            var result = await Mediator.Send(new GetPagedPostListQuery { Page = 1 });

            result.IsSuccess.Should().BeTrue();
            result.Value.Posts.Should().HaveCount(3);
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_With3Posts_WithItemsPerPage2()
        {
            await SeedDatabase();

            var result = await Mediator.Send(new GetPagedPostListQuery { Page = 1, ItemsPerPage = 2 });

            result.IsSuccess.Should().BeTrue();
            result.Value.Posts.Should().HaveCount(2);
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_With2Posts()
        {
            await SeedDatabase();

            var result = await Mediator.Send(new GetPagedPostListQuery { Page = 2 });

            result.IsSuccess.Should().BeTrue();
            result.Value.Posts.Should().HaveCount(2);
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_With6Posts_WhenIncludingUnpublishedPosts()
        {
            await SeedDatabase();

            var result = await Mediator.Send(new GetPagedPostListQuery { Page = 1, IncludeUnpublished = true, ItemsPerPage = 100 });

            result.IsSuccess.Should().BeTrue();
            result.Value.Posts.Should().HaveCount(6);
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_WithBlogCoverUrlFormatReplaced()
        {
            await SeedDatabase();

            var result = await Mediator.Send(new GetPagedPostListQuery());

            result.IsSuccess.Should().BeTrue();
            result.Value.Blog.CoverUrl.Should().Be("http://127.0.0.1:10000/devstoreaccount1/pineblog-tests/blog/cover-image.png");
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_WithPostListTypeBlog()
        {
            await SeedDatabase();

            var result = await Mediator.Send(new GetPagedPostListQuery());

            result.IsSuccess.Should().BeTrue();
            result.Value.PostListType.Should().Be(PostListType.Blog);
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_WhenFilterOnCategory_WithPostListTypeCategory()
        {
            await SeedDatabase();

            var result = await Mediator.Send(new GetPagedPostListQuery { Category = "category" });

            result.IsSuccess.Should().BeTrue();
            result.Value.PostListType.Should().Be(PostListType.Category);
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_WithBlogInfo()
        {
            await SeedDatabase();

            var result = await Mediator.Send(new GetPagedPostListQuery());

            result.IsSuccess.Should().BeTrue();
            result.Value.Posts.Should().HaveCount(3);
            result.Value.Blog.Title.Should().Be("Title from configuration");
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_WithPostCovers()
        {
            await SeedDatabase();

            var result = await Mediator.Send(new GetPagedPostListQuery());

            result.IsSuccess.Should().BeTrue();
            result.Value.Posts.Should().HaveCount(3);
            result.Value.Posts.First().CoverUrl.Should().Be("https://ofpinewood.com/cover-url");
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_WithPostAuthors()
        {
            await SeedDatabase();

            var result = await Mediator.Send(new GetPagedPostListQuery());

            result.IsSuccess.Should().BeTrue();
            result.Value.Posts.Should().HaveCount(3);
            result.Value.Posts.First().Author.DisplayName.Should().Be("Author 1");
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_WithPager()
        {
            await SeedDatabase();

            var result = await Mediator.Send(new GetPagedPostListQuery());

            result.IsSuccess.Should().BeTrue();
            result.Value.Pager.CurrentPage.Should().Be(1);
            result.Value.Pager.ItemsPerPage.Should().Be(3);
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_With3Posts_ForCategoryCat2()
        {
            await SeedDatabase();

            var result = await Mediator.Send(new GetPagedPostListQuery { Page = 1, Category = "cat2", ItemsPerPage = 100 });

            result.IsSuccess.Should().BeTrue();
            result.Value.Posts.Should().HaveCount(3);
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_With1Post_ForCategoryCat3()
        {
            await SeedDatabase();

            var result = await Mediator.Send(new GetPagedPostListQuery { Page = 1, Category = "cat3", ItemsPerPage = 100 });

            result.IsSuccess.Should().BeTrue();
            result.Value.Posts.Should().HaveCount(1);
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_With2Posts_ForCategoryCat3IncludingUnpublishedPosts()
        {
            await SeedDatabase();

            var result = await Mediator.Send(new GetPagedPostListQuery { Page = 1, IncludeUnpublished = true, Category = "cat3", ItemsPerPage = 100 });

            result.IsSuccess.Should().BeTrue();
            result.Value.Posts.Should().HaveCount(2);
        }

        private async Task SeedDatabase()
        {
            var repo = ServiceProvider.GetRequiredService<IRepository>();
            CancellationToken cancelToken = CancellationToken.None;

            var author = new Author { UserName = "user@example.com", DisplayName = "Author 1" };
            await repo.AddAuthorAsync(author, cancelToken);

            await repo.AddPostAsync(CreatePost(0, author.Id, true, false, "cat1"), cancelToken);
            await repo.AddPostAsync(CreatePost(1, author.Id, true, true, "cat1"), cancelToken);
            await repo.AddPostAsync(CreatePost(2, author.Id, true, true, "cat1,cat2"), cancelToken);
            await repo.AddPostAsync(CreatePost(3, author.Id, true, true, "cat2"), cancelToken);
            await repo.AddPostAsync(CreatePost(4, author.Id, true, true, "cat1,cat2,cat3"), cancelToken);
            await repo.AddPostAsync(CreatePost(5, author.Id, false, true, "cat3"), cancelToken);
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
