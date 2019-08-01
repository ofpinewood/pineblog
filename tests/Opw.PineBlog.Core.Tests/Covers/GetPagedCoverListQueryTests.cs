using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Opw.PineBlog.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.Covers
{
    public class GetPagedCoverListQueryTests : MediatRTestsBase
    {
        public GetPagedCoverListQueryTests() : base()
        {
            SeedDatabase();
        }

        [Fact]
        public async Task Handler_Should_ReturnCoverListModel_With3Covers()
        {
            var result = await Mediator.Send(new GetPagedCoverListQuery { Page = 1 });

            result.IsSuccess.Should().BeTrue();
            result.Value.Covers.Should().HaveCount(3);
        }

        [Fact]
        public async Task Handler_Should_ReturnCoverListModel_With3Covers_WithItemsPerPage2()
        {
            var result = await Mediator.Send(new GetPagedCoverListQuery { Page = 1, ItemsPerPage = 2 });

            result.IsSuccess.Should().BeTrue();
            result.Value.Covers.Should().HaveCount(2);
        }

        [Fact]
        public async Task Handler_Should_ReturnCoverListModel_With2Covers()
        {
            var result = await Mediator.Send(new GetPagedCoverListQuery { Page = 2 });

            result.IsSuccess.Should().BeTrue();
            result.Value.Covers.Should().HaveCount(2);
        }

        [Fact]
        public async Task Handler_Should_ReturnCoverListModel_With1Cover_With2Posts()
        {
            var result = await Mediator.Send(new GetPagedCoverListQuery { Page = 1, ItemsPerPage = 10 });

            result.IsSuccess.Should().BeTrue();
            result.Value.Covers.Last().Post.Should().NotBeNull();
        }

        [Fact]
        public async Task Handler_Should_ReturnCoverListModel_WithPager()
        {
            var result = await Mediator.Send(new GetPagedCoverListQuery());

            result.IsSuccess.Should().BeTrue();
            result.Value.Pager.CurrentPage.Should().Be(1);
            result.Value.Pager.ItemsPerPage.Should().Be(3);
        }

        private void SeedDatabase()
        {
            var context = ServiceProvider.GetRequiredService<IBlogEntityDbContext>();

            var author = new Author { UserName = "user@example.com", DisplayName = "Author 1" };
            context.Authors.Add(author);
            context.SaveChanges();

            context.Posts.Add(CreatePost(0, author.Id));
            context.Posts.Add(CreatePost(1, author.Id));
            context.Posts.Add(CreatePost(2, author.Id));
            context.Posts.Add(CreatePost(3, author.Id));
            context.Posts.Add(CreatePost(4, author.Id));
            context.SaveChanges();
        }

        private Post CreatePost(int i, Guid authorId)
        {
            return new Post
            {
                AuthorId = authorId,
                Title = "Post title " + i,
                Slug = "post-title-" + i,
                Description = "Description",
                Content = "Content",
                Cover = new Cover
                {
                    Url = "https://ofpinewood.com/cover-url-" + i,
                    Caption = "Cover " + i,
                    Link = "https://ofpinewood.com/cover-link-" + i
                },
                Published = DateTime.UtcNow
            };
        }
    }
}
