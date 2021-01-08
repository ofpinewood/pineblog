using FluentAssertions;
using Moq;
using Opw.PineBlog.Entities;
using Opw.PineBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.Posts
{
    public class GetPagedPostListQueryTests : MediatRTestsBase
    {
        private Guid _authorId = Guid.NewGuid();

        public GetPagedPostListQueryTests() : base()
        {
            var author = new Author { Id = _authorId, UserName = "user@example.com", DisplayName = "Author 1" };

            var posts = new List<Post>();
            posts.Add(CreatePost(0, author, false, "cat1"));
            posts.Add(CreatePost(1, author, true, "cat1"));
            posts.Add(CreatePost(2, author, true, "cat1"));
            posts.Add(CreatePost(3, author, true, "cat1"));
            posts.Add(CreatePost(4, author, true, "cat1"));

            PostRepositoryMock.Setup(m => m.GetAsync(It.IsAny<IEnumerable<Expression<Func<Post, bool>>>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(posts);
            PostRepositoryMock.Setup(m => m.CountAsync(It.IsAny<IEnumerable<Expression<Func<Post, bool>>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(posts.Count);
        }

        [Fact]
        public async Task Handler_Should_GetPosts_ForPage1()
        {
            var result = await Mediator.Send(new GetPagedPostListQuery { Page = 1 });

            result.IsSuccess.Should().BeTrue();

            PostRepositoryMock.Verify(m => m.GetAsync(It.IsAny<IEnumerable<Expression<Func<Post, bool>>>>(), 0, It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handler_Should_GetPosts_ForPage1_WithItemsPerPage2()
        {
            var result = await Mediator.Send(new GetPagedPostListQuery { Page = 1, ItemsPerPage = 2 });

            result.IsSuccess.Should().BeTrue();

            PostRepositoryMock.Verify(m => m.GetAsync(It.IsAny<IEnumerable<Expression<Func<Post, bool>>>>(), 0, 2, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handler_Should_GetPosts_ForPage2()
        {
            var result = await Mediator.Send(new GetPagedPostListQuery { Page = 2 });

            result.IsSuccess.Should().BeTrue();

            PostRepositoryMock.Verify(m => m.GetAsync(It.IsAny<IEnumerable<Expression<Func<Post, bool>>>>(), 3, It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handler_Should_Predicates_PublishedExpression_WhenNotIncludingUnpublishedPosts()
        {
            IEnumerable<Expression<Func<Post, bool>>> createdPredicates = new List<Expression<Func<Post, bool>>>();

            PostRepositoryMock
                .Setup(m => m.GetAsync(It.IsAny<IEnumerable<Expression<Func<Post, bool>>>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Callback((IEnumerable<Expression<Func<Post, bool>>> predicates, int _, int __, CancellationToken ___) => createdPredicates = predicates);

            var result = await Mediator.Send(new GetPagedPostListQuery { Page = 1, IncludeUnpublished = false, ItemsPerPage = 100 });

            result.IsSuccess.Should().BeTrue();

            Expression<Func<Post, bool>> publishedExpression = p => p.Published != null;
            createdPredicates.Should().ContainEquivalentOf(publishedExpression);
        }

        [Fact]
        public async Task Handler_Should_Predicates_Empty_WhenIncludingUnpublishedPosts()
        {
            IEnumerable<Expression<Func<Post, bool>>> createdPredicates = new List<Expression<Func<Post, bool>>>();

            PostRepositoryMock
                .Setup(m => m.GetAsync(It.IsAny<IEnumerable<Expression<Func<Post, bool>>>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Callback((IEnumerable<Expression<Func<Post, bool>>> predicates, int _, int __, CancellationToken ___) => createdPredicates = predicates);

            var result = await Mediator.Send(new GetPagedPostListQuery { Page = 1, IncludeUnpublished = true, ItemsPerPage = 100 });

            result.IsSuccess.Should().BeTrue();

            Expression<Func<Post, bool>> publishedExpression = p => p.Published != null;
            createdPredicates.Should().BeEmpty();
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
            result.Value.Blog.Title.Should().Be("Title from configuration");
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_WithPostCovers()
        {
            var result = await Mediator.Send(new GetPagedPostListQuery());

            result.IsSuccess.Should().BeTrue();
            result.Value.Posts.First().CoverUrl.Should().BeNull();
            result.Value.Posts.Last().CoverUrl.Should().NotBeNull();
        }

        [Fact]
        public async Task Handler_Should_CoverUrl_ReplaceBaseUrlWithUrlFormat()
        {
            var result = await Mediator.Send(new GetPagedPostListQuery());

            result.IsSuccess.Should().BeTrue();

            result.Value.Should().NotBeNull();
            result.Value.Posts.Last().CoverUrl.Should().Be("http://127.0.0.1:10000/devstoreaccount1/pineblog-tests/cover-url");
        }

        [Fact]
        public async Task Handler_Should_UrlsInContent_ReplaceBaseUrlWithUrlFormat()
        {
            var result = await Mediator.Send(new GetPagedPostListQuery());

            result.IsSuccess.Should().BeTrue();

            result.Value.Should().NotBeNull();
            result.Value.Posts.Last().Content.Should().Be("Content with a url: http://127.0.0.1:10000/devstoreaccount1/pineblog-tests/content-url");
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_WithPostAuthors()
        {
            var result = await Mediator.Send(new GetPagedPostListQuery());

            result.IsSuccess.Should().BeTrue();
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
        public async Task Handler_Should_Predicates_CategoryExpression_ForCategoryCat2()
        {
            IEnumerable<Expression<Func<Post, bool>>> createdPredicates = new List<Expression<Func<Post, bool>>>();

            PostRepositoryMock
                .Setup(m => m.GetAsync(It.IsAny<IEnumerable<Expression<Func<Post, bool>>>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Callback((IEnumerable<Expression<Func<Post, bool>>> predicates, int _, int __, CancellationToken ___) => createdPredicates = predicates);

            var result = await Mediator.Send(new GetPagedPostListQuery { Page = 1, Category = "cat2", ItemsPerPage = 100 });

            result.IsSuccess.Should().BeTrue();

            Expression<Func<Post, bool>> categoryExpression = p => p.Categories.Contains("cat2");
            createdPredicates.Should().ContainEquivalentOf(categoryExpression);
        }

        [Fact]
        public async Task Handler_Should_Predicates_CategoryExpressionAndPublishedExpression_ForCategoryCat2AndIncludeUnpublishedFalse()
        {
            IEnumerable<Expression<Func<Post, bool>>> createdPredicates = new List<Expression<Func<Post, bool>>>();

            PostRepositoryMock
                .Setup(m => m.GetAsync(It.IsAny<IEnumerable<Expression<Func<Post, bool>>>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Callback((IEnumerable<Expression<Func<Post, bool>>> predicates, int _, int __, CancellationToken ___) => createdPredicates = predicates);

            var result = await Mediator.Send(new GetPagedPostListQuery { Page = 1, IncludeUnpublished = false, Category = "cat3", ItemsPerPage = 100 });

            result.IsSuccess.Should().BeTrue();

            Expression<Func<Post, bool>> categoryExpression = p => p.Categories.Contains("cat2");
            createdPredicates.Should().ContainEquivalentOf(categoryExpression);
            Expression<Func<Post, bool>> publishedExpression = p => p.Published != null;
            createdPredicates.Should().ContainEquivalentOf(publishedExpression);
        }

        private Post CreatePost(int i, Author author, bool cover, string categories)
        {
            var post = new Post
            {
                Author = author,
                Title = "Post title " + i,
                Slug = "post-title-" + i,
                Categories = categories,
                Description = "Description",
                Content = "Content with a url: %URL%/content-url",
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow,
                Published = DateTime.UtcNow
            };

            if (cover)
            {
                post.CoverUrl = "%URL%/cover-url";
                post.CoverCaption = "Cover caption";
                post.CoverLink = "https://ofpinewood.com/cover-link";
            }

            return post;
        }
    }
}
