using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Opw.PineBlog.Entities;
using Opw.PineBlog.Files;
using Opw.PineBlog.MongoDb;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.Posts
{
    public class SearchPostsQueryTests : MongoDbTestsBase
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

        [Fact(Skip = Constants.SkipMongoDbTests)]
        public async Task Handler_Should_Return6Posts_MatchOnTitle()
        {
            var result = await searchPostsQueryHandler.Handle(new SearchPostsQuery { Page = 1, SearchQuery = "title", ItemsPerPage = 100 }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Posts.Should().HaveCount(6);
        }

        [Fact(Skip = Constants.SkipMongoDbTests)]
        public async Task Handler_Should_Return1Post_MatchOnTitle()
        {
            var result = await searchPostsQueryHandler.Handle(new SearchPostsQuery { Page = 1, SearchQuery = "title1", ItemsPerPage = 100 }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Posts.Should().HaveCount(1);
        }

        [Fact(Skip = Constants.SkipMongoDbTests)]
        public async Task Handler_Should_Return6Post_MatchOnCategories()
        {
            var result = await searchPostsQueryHandler.Handle(new SearchPostsQuery { Page = 1, SearchQuery = "categories", ItemsPerPage = 100 }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Posts.Should().HaveCount(6);
        }

        [Fact(Skip = Constants.SkipMongoDbTests)]
        public async Task Handler_Should_Return2Post_MatchOnCategories()
        {
            var result = await searchPostsQueryHandler.Handle(new SearchPostsQuery { Page = 1, SearchQuery = "catc", ItemsPerPage = 100 }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Posts.Should().HaveCount(2);
        }

        [Fact(Skip = Constants.SkipMongoDbTests)]
        public async Task Handler_Should_Return6Post_MatchOnDescription()
        {
            var result = await searchPostsQueryHandler.Handle(new SearchPostsQuery { Page = 1, SearchQuery = "description", ItemsPerPage = 100 }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Posts.Should().HaveCount(6);
        }

        [Fact(Skip = Constants.SkipMongoDbTests)]
        public async Task Handler_Should_Return1Post_MatchOnDescription()
        {
            var result = await searchPostsQueryHandler.Handle(new SearchPostsQuery { Page = 1, SearchQuery = "description1", ItemsPerPage = 100 }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Posts.Should().HaveCount(1);
        }

        [Fact(Skip = Constants.SkipMongoDbTests)]
        public async Task Handler_Should_Return6Post_MatchOnContent()
        {
            var result = await searchPostsQueryHandler.Handle(new SearchPostsQuery { Page = 1, SearchQuery = "content", ItemsPerPage = 100 }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Posts.Should().HaveCount(6);
        }

        [Fact(Skip = Constants.SkipMongoDbTests)]
        public async Task Handler_Should_Return1Post_MatchOnContent()
        {
            var result = await searchPostsQueryHandler.Handle(new SearchPostsQuery { Page = 1, SearchQuery = "content1", ItemsPerPage = 100 }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Posts.Should().HaveCount(1);
        }

        [Fact(Skip = Constants.SkipMongoDbTests)]
        public async Task Handler_Should_Return2Post_MultipleTerms_MatchOnTitle()
        {
            var result = await searchPostsQueryHandler.Handle(new SearchPostsQuery { Page = 1, SearchQuery = "title1 title2", ItemsPerPage = 100 }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Posts.Should().HaveCount(2);
        }

        [Fact(Skip = Constants.SkipMongoDbTests)]
        public async Task Handler_Should_Return2Post_MultipleTerms_MatchOnTitleAndDescription()
        {
            var result = await searchPostsQueryHandler.Handle(new SearchPostsQuery { Page = 1, SearchQuery = "title1 description2", ItemsPerPage = 100 }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Posts.Should().HaveCount(2);
        }

        [Fact(Skip = Constants.SkipMongoDbTests)]
        public async Task Handler_Should_Return3Post_MultipleTerms_MatchOnTitleAndCategories()
        {
            var result = await searchPostsQueryHandler.Handle(new SearchPostsQuery { Page = 1, SearchQuery = "title1 catc", ItemsPerPage = 100 }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Posts.Should().HaveCount(3);
        }

        private void SeedDatabase()
        {
            var authorId = Guid.NewGuid();
            AuthorCollection.InsertOne(new Author
            {
                Id = authorId,
                DisplayName = "Author 1",
                UserName = "user@example.com"
            });

            PostCollection.InsertOne(CreatePost(0, authorId, "cata"));
            PostCollection.InsertOne(CreatePost(1, authorId, "cata"));
            PostCollection.InsertOne(CreatePost(2, authorId, "cata,catb"));
            PostCollection.InsertOne(CreatePost(3, authorId, "catb"));
            PostCollection.InsertOne(CreatePost(4, authorId, "cata,catb,catc"));
            PostCollection.InsertOne(CreatePost(5, authorId, "catc"));
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
