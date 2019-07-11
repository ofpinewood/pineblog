using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Opw.PineBlog.Entities;
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
            var result = await Mediator.Send(new GetPagedPostListQuery { Page = 0 });

            result.IsSuccess.Should().BeTrue();
            result.Value.Posts.Should().HaveCount(3);
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_With2Posts()
        {
            var result = await Mediator.Send(new GetPagedPostListQuery { Page = 1 });

            result.IsSuccess.Should().BeTrue();
            result.Value.Posts.Should().HaveCount(2);
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
            result.Value.Posts.First().Cover.Url.Should().Be("https://ofpinewood.com/cover-url");
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
            result.Value.Pager.CurrentPage.Should().Be(0);
            result.Value.Pager.ItemsPerPage.Should().Be(3);
        }

        private void SeedDatabase()
        {
            var context = ServiceProvider.GetRequiredService<IBlogEntityDbContext>();

            var author = new Author { UserId = Guid.NewGuid(), DisplayName = "Author 1" };
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
                Published = DateTime.UtcNow,
                Cover = new Cover
                {
                    Url = "https://ofpinewood.com/cover-url",
                    Caption = "Cover caption",
                    Link = "https://ofpinewood.com/cover-link"
                }
            };
        }
    }
}
