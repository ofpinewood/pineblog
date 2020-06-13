using FluentAssertions;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Opw.HttpExceptions;
using Opw.PineBlog.Entities;
using System;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Xunit;

namespace Opw.PineBlog.Feeds
{
    public class GetSyndicationFeedQueryTests : MediatRTestsBase
    {
        public GetSyndicationFeedQueryTests() : base()
        {
        }

        [Fact]
        public async Task Validator_Should_ThrowValidationErrorException()
        {
            await SeedDatabase();

            Task action() => Mediator.Send(new GetSyndicationFeedQuery());

            var ex = await Assert.ThrowsAsync<ValidationErrorException<ValidationFailure>>(action);
            ex.Errors.Single(e => e.Key.Equals(nameof(GetSyndicationFeedQuery.BaseUri))).Should().NotBeNull();
            ex.Errors.Single(e => e.Key.Equals(nameof(GetSyndicationFeedQuery.FeedType))).Should().NotBeNull();
            ex.Errors.Single(e => e.Key.Equals(nameof(GetSyndicationFeedQuery.PostBasePath))).Should().NotBeNull();
        }

        [Fact]
        public async Task Handler_Should_ReturnRssFeed_WhenFeedTypeRss()
        {
            await SeedDatabase();

            var result = await Mediator.Send(new GetSyndicationFeedQuery
            {
                BaseUri = new Uri("http://www.example.com"),
                FeedType = FeedType.Rss,
                PostBasePath = "posts"
            });

            result.IsSuccess.Should().BeTrue();
            result.Value.ContentType.Should().Be("application/rss+xml");
            result.Value.Feed.Should().StartWith("<rss xmlns:a10=\"http://www.w3.org/2005/Atom\" version=\"2.0\">");
        }

        [Fact]
        public async Task Handler_Should_ReturnAtomFeed_WhenFeedTypeAtom()
        {
            await SeedDatabase();

            var result = await Mediator.Send(new GetSyndicationFeedQuery
            {
                BaseUri = new Uri("http://www.example.com"),
                FeedType = FeedType.Atom,
                PostBasePath = "posts"
            });

            result.IsSuccess.Should().BeTrue();
            result.Value.ContentType.Should().Be("application/atom+xml");
            result.Value.Feed.Should().StartWith("<feed xml:base=\"http://www.example.com/\" xmlns=\"http://www.w3.org/2005/Atom\">");
        }

        [Fact]
        public async Task Handler_Should_ReturnFeedModel_With5Posts()
        {
            await SeedDatabase();

            var result = await Mediator.Send(new GetSyndicationFeedQuery
            {
                BaseUri = new Uri("http://www.example.com"),
                FeedType = FeedType.Rss,
                PostBasePath = "posts"
            });

            result.IsSuccess.Should().BeTrue();

            var feedXml = new XmlDocument();
            feedXml.LoadXml(result.Value.Feed);

            var items = feedXml.SelectNodes("/rss/channel/item");
            items.Should().HaveCount(5);
        }

        [Fact]
        public async Task Handler_Should_ReturnFeedModel_WithCorrectItemTitle()
        {
            await SeedDatabase();

            var result = await Mediator.Send(new GetSyndicationFeedQuery
            {
                BaseUri = new Uri("http://www.example.com"),
                FeedType = FeedType.Rss,
                PostBasePath = "posts"
            });

            result.IsSuccess.Should().BeTrue();

            var feedXml = new XmlDocument();
            feedXml.LoadXml(result.Value.Feed);

            var items = feedXml.SelectNodes("/rss/channel/item");
            var title = items[0].SelectSingleNode("title").InnerText;
            title.Should().Be("Post title 4");
        }

        [Fact]
        public async Task Handler_Should_ReturnFeedModel_WithCorrectItemId()
        {
            await SeedDatabase();

            var result = await Mediator.Send(new GetSyndicationFeedQuery
            {
                BaseUri = new Uri("http://www.example.com"),
                FeedType = FeedType.Rss,
                PostBasePath = "posts"
            });

            result.IsSuccess.Should().BeTrue();

            var feedXml = new XmlDocument();
            feedXml.LoadXml(result.Value.Feed);

            var items = feedXml.SelectNodes("/rss/channel/item");
            var guid = items[0].SelectSingleNode("guid").InnerText;
            guid.Should().Be("http://www.example.com/posts/post-title-4");
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
