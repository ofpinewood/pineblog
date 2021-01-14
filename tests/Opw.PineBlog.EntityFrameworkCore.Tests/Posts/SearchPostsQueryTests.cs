using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Opw.PineBlog.Entities;
using Opw.PineBlog.EntityFrameworkCore;
using Opw.PineBlog.Files;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.Posts
{
    public class SearchPostsQueryTests : EntityFrameworkCoreTestsBase
    {
        private readonly SearchPostsQuery.Handler searchPostsQueryHandler;

        public SearchPostsQueryTests()
        {
            SeedDatabase();

            var uow = ServiceProvider.GetRequiredService<IBlogUnitOfWork>();
            var options = ServiceProvider.GetRequiredService<IOptionsSnapshot<PineBlogOptions>>();
            var postUrlHelper = ServiceProvider.GetRequiredService<PostUrlHelper>();
            var fileUrlHelper = ServiceProvider.GetRequiredService<FileUrlHelper>();

            searchPostsQueryHandler = new SearchPostsQuery.Handler(uow, options, postUrlHelper, fileUrlHelper);
        }

        [Fact]
        public async Task Handler_Should_Return6Posts_MatchOnTitle()
        {
            var result = await searchPostsQueryHandler.Handle(new SearchPostsQuery { Page = 1, SearchQuery = "title", ItemsPerPage = 100 }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Posts.Should().HaveCount(6);
        }

        [Fact]
        public async Task Handler_Should_Return1Post_MatchOnTitle()
        {
            var result = await searchPostsQueryHandler.Handle(new SearchPostsQuery { Page = 1, SearchQuery = "title1", ItemsPerPage = 100 }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Posts.Should().HaveCount(1);
        }

        [Fact]
        public async Task Handler_Should_Return6Post_MatchOnCategories()
        {
            var result = await searchPostsQueryHandler.Handle(new SearchPostsQuery { Page = 1, SearchQuery = "categories", ItemsPerPage = 100 }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Posts.Should().HaveCount(6);
        }

        [Fact]
        public async Task Handler_Should_Return2Post_MatchOnCategories()
        {
            var result = await searchPostsQueryHandler.Handle(new SearchPostsQuery { Page = 1, SearchQuery = "catc", ItemsPerPage = 100 }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Posts.Should().HaveCount(2);
        }

        [Fact]
        public async Task Handler_Should_Return6Post_MatchOnDescription()
        {
            var result = await searchPostsQueryHandler.Handle(new SearchPostsQuery { Page = 1, SearchQuery = "description", ItemsPerPage = 100 }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Posts.Should().HaveCount(6);
        }

        [Fact]
        public async Task Handler_Should_Return1Post_MatchOnDescription()
        {
            var result = await searchPostsQueryHandler.Handle(new SearchPostsQuery { Page = 1, SearchQuery = "description1", ItemsPerPage = 100 }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Posts.Should().HaveCount(1);
        }

        [Fact]
        public async Task Handler_Should_Return6Post_MatchOnContent()
        {
            var result = await searchPostsQueryHandler.Handle(new SearchPostsQuery { Page = 1, SearchQuery = "content", ItemsPerPage = 100 }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Posts.Should().HaveCount(6);
        }

        [Fact]
        public async Task Handler_Should_Return1Post_MatchOnContent()
        {
            var result = await searchPostsQueryHandler.Handle(new SearchPostsQuery { Page = 1, SearchQuery = "content1", ItemsPerPage = 100 }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Posts.Should().HaveCount(1);
        }

        [Fact]
        public async Task Handler_Should_Return2Post_MultipleTerms_MatchOnTitle()
        {
            var result = await searchPostsQueryHandler.Handle(new SearchPostsQuery { Page = 1, SearchQuery = "title1 title2", ItemsPerPage = 100 }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Posts.Should().HaveCount(2);
        }

        [Fact]
        public async Task Handler_Should_Return2Post_MultipleTerms_MatchOnTitleAndDescription()
        {
            var result = await searchPostsQueryHandler.Handle(new SearchPostsQuery { Page = 1, SearchQuery = "title1 description2", ItemsPerPage = 100 }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Posts.Should().HaveCount(2);
        }

        [Fact]
        public async Task Handler_Should_Return3Post_MultipleTerms_MatchOnTitleAndCategories()
        {
            var result = await searchPostsQueryHandler.Handle(new SearchPostsQuery { Page = 1, SearchQuery = "title1 catc", ItemsPerPage = 100 }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Posts.Should().HaveCount(3);
        }

        private void SeedDatabase()
        {
            var context = ServiceProvider.GetRequiredService<BlogEntityDbContext>();

            var author = new Author { UserName = "user@example.com", DisplayName = "Author 1" };
            context.Authors.Add(author);
            context.SaveChanges();

            context.Posts.Add(CreatePost(0, author.Id, "cata"));
            context.Posts.Add(CreatePost(1, author.Id, "cata"));
            context.Posts.Add(CreatePost(2, author.Id, "cata,catb"));
            context.Posts.Add(CreatePost(3, author.Id, "catb"));
            context.Posts.Add(CreatePost(4, author.Id, "cata,catb,catc"));
            context.Posts.Add(CreatePost(5, author.Id, "catc"));
            context.SaveChanges();
        }

        private Post CreatePost(int i, Guid authorId, string categories)
        {
            return new Post
            {
                AuthorId = authorId,
                Title = "title aaa title" + i,
                Slug = "title-aaa-" + i,
                Categories = $"categories,{categories}",
                Description = "description aaa description" + i,
                Content = "content ccc content" + i,
                Published = DateTime.UtcNow.AddDays(-i)
            };
        }
    }
}
