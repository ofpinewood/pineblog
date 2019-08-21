using FluentAssertions;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Opw.HttpExceptions;
using Opw.PineBlog.Entities;
using System;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.Feeds
{
    public class GetSyndicationFeedQueryTests : MediatRTestsBase
    {
        public GetSyndicationFeedQueryTests() : base()
        {
            SeedDatabase();
        }

        [Fact]
        public async Task Validator_Should_ThrowValidationErrorException()
        {
            Task action() => Mediator.Send(new GetSyndicationFeedQuery());

            var ex = await Assert.ThrowsAsync<ValidationErrorException<ValidationFailure>>(action);
            ex.Errors.Single(e => e.Key.Equals(nameof(GetSyndicationFeedQuery.BaseUri))).Should().NotBeNull();
            ex.Errors.Single(e => e.Key.Equals(nameof(GetSyndicationFeedQuery.FeedType))).Should().NotBeNull();
            ex.Errors.Single(e => e.Key.Equals(nameof(GetSyndicationFeedQuery.PostBasePath))).Should().NotBeNull();
        }

        [Fact]
        public async Task Handler_Should_ReturnFeedModel_With5Posts()
        {
            var result = await Mediator.Send(new GetSyndicationFeedQuery {
                BaseUri = new Uri("http://www.example.com"),
                FeedType = FeedType.Rss,
                PostBasePath = "posts"
            });

            result.IsSuccess.Should().BeTrue();
            result.Value.Feed.Feed.Items.Should().HaveCount(5);
        }

        [Fact]
        public async Task Handler_Should_ReturnFeedModel_WithFeedOfTypeRss20FeedFormatter()
        {
            var result = await Mediator.Send(new GetSyndicationFeedQuery
            {
                BaseUri = new Uri("http://www.example.com"),
                FeedType = FeedType.Rss,
                PostBasePath = "posts"
            });

            result.IsSuccess.Should().BeTrue();
            result.Value.ContentType.Should().Be("application/rss+xml");
            result.Value.Feed.Should().BeOfType<Rss20FeedFormatter>();
        }

        [Fact]
        public async Task Handler_Should_ReturnFeedModel_WithFeedOfTypeAtom10FeedFormatter()
        {
            var result = await Mediator.Send(new GetSyndicationFeedQuery
            {
                BaseUri = new Uri("http://www.example.com"),
                FeedType = FeedType.Atom,
                PostBasePath = "posts"
            });

            result.IsSuccess.Should().BeTrue();
            result.Value.ContentType.Should().Be("application/atom+xml");
            result.Value.Feed.Should().BeOfType<Atom10FeedFormatter>();
        }

        [Fact]
        public async Task Handler_Should_ReturnFeedModel_WithCorrectItemTitle()
        {
            var result = await Mediator.Send(new GetSyndicationFeedQuery
            {
                BaseUri = new Uri("http://www.example.com"),
                FeedType = FeedType.Rss,
                PostBasePath = "posts"
            });

            result.IsSuccess.Should().BeTrue();
            result.Value.Feed.Feed.Items.First().Title.Text.Should().Be("Post title 4");
        }

        [Fact]
        public async Task Handler_Should_ReturnFeedModel_WithCorrectItemId()
        {
            var result = await Mediator.Send(new GetSyndicationFeedQuery
            {
                BaseUri = new Uri("http://www.example.com"),
                FeedType = FeedType.Rss,
                PostBasePath = "posts"
            });

            result.IsSuccess.Should().BeTrue();
            result.Value.Feed.Feed.Items.First().Id.Should().Be("http://www.example.com/posts/post-title-4");
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
