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
    public class SearchPostsQueryTests : MediatRTestsBase
    {
        private Guid _authorId = Guid.NewGuid();

        public SearchPostsQueryTests()
        {
            var author = new Author { Id = _authorId, UserName = "user@example.com", DisplayName = "Author 1" };

            var posts = new List<Post>();
            posts.Add(CreatePost(0, author, false));
            posts.Add(CreatePost(1, author, true));
            posts.Add(CreatePost(2, author, true));
            posts.Add(CreatePost(3, author, true));
            posts.Add(CreatePost(4, author, true));

            PostRepositoryMock.Setup(m => m.GetAsync(It.IsAny<IEnumerable<Expression<Func<Post, bool>>>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(posts);
            PostRepositoryMock.Setup(m => m.CountAsync(It.IsAny<IEnumerable<Expression<Func<Post, bool>>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(posts.Count);
        }

        [Fact]
        public async Task Handler_Should_GetPosts_ForPage1()
        {
            var result = await Mediator.Send(new SearchPostsQuery { Page = 1 });

            result.IsSuccess.Should().BeTrue();

            PostRepositoryMock.Verify(m => m.GetAsync(It.IsAny<IEnumerable<Expression<Func<Post, bool>>>>(), 0, It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handler_Should_GetPosts_ForPage1_WithItemsPerPage2()
        {
            var result = await Mediator.Send(new SearchPostsQuery { Page = 1, ItemsPerPage = 2 });

            result.IsSuccess.Should().BeTrue();

            PostRepositoryMock.Verify(m => m.GetAsync(It.IsAny<IEnumerable<Expression<Func<Post, bool>>>>(), 0, 2, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handler_Should_GetPosts_ForPage2()
        {
            var result = await Mediator.Send(new SearchPostsQuery { Page = 2 });

            result.IsSuccess.Should().BeTrue();

            PostRepositoryMock.Verify(m => m.GetAsync(It.IsAny<IEnumerable<Expression<Func<Post, bool>>>>(), 3, It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_WithBlogCoverUrlFormatReplaced()
        {
            var result = await Mediator.Send(new SearchPostsQuery());

            result.IsSuccess.Should().BeTrue();
            result.Value.Blog.CoverUrl.Should().Be("http://127.0.0.1:10000/devstoreaccount1/pineblog-tests/blog/cover-image.png");
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_WithPostListTypeBlog()
        {
            var result = await Mediator.Send(new SearchPostsQuery());

            result.IsSuccess.Should().BeTrue();
            result.Value.PostListType.Should().Be(PostListType.Blog);
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_WithPostListTypeSearch()
        {
            var result = await Mediator.Send(new SearchPostsQuery { SearchQuery = "text" });

            result.IsSuccess.Should().BeTrue();
            result.Value.PostListType.Should().Be(PostListType.Search);
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_WithPostListSearchQuery()
        {
            var result = await Mediator.Send(new SearchPostsQuery { SearchQuery = "text" });

            result.IsSuccess.Should().BeTrue();
            result.Value.SearchQuery.Should().Be("text");
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_WithBlogInfo()
        {
            var result = await Mediator.Send(new SearchPostsQuery());

            result.IsSuccess.Should().BeTrue();
            result.Value.Blog.Title.Should().Be("Title from configuration");
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_WithPostCovers()
        {
            var result = await Mediator.Send(new SearchPostsQuery());

            result.IsSuccess.Should().BeTrue();
            result.Value.Posts.First().CoverUrl.Should().BeNull();
            result.Value.Posts.Last().CoverUrl.Should().NotBeNull();
        }

        [Fact]
        public async Task Handler_Should_CoverUrl_ReplaceBaseUrlWithUrlFormat()
        {
            var result = await Mediator.Send(new SearchPostsQuery());

            result.IsSuccess.Should().BeTrue();

            result.Value.Should().NotBeNull();
            result.Value.Posts.Last().CoverUrl.Should().Be("http://127.0.0.1:10000/devstoreaccount1/pineblog-tests/cover-url");
        }

        [Fact]
        public async Task Handler_Should_UrlsInContent_ReplaceBaseUrlWithUrlFormat()
        {
            var result = await Mediator.Send(new SearchPostsQuery());

            result.IsSuccess.Should().BeTrue();

            result.Value.Should().NotBeNull();
            result.Value.Posts.Last().Content.Should().Be("content with a url: http://127.0.0.1:10000/devstoreaccount1/pineblog-tests/content-url");
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_WithPostAuthors()
        {
            var result = await Mediator.Send(new SearchPostsQuery());

            result.IsSuccess.Should().BeTrue();
            result.Value.Posts.First().Author.DisplayName.Should().Be("Author 1");
        }

        [Fact]
        public async Task Handler_Should_ReturnPostListModel_WithPager()
        {
            var result = await Mediator.Send(new SearchPostsQuery());

            result.IsSuccess.Should().BeTrue();
            result.Value.Pager.CurrentPage.Should().Be(1);
            result.Value.Pager.ItemsPerPage.Should().Be(3);
        }

        [Fact]
        public async Task Handler_Should_Predicates_SearchExpression_WhenGetPosts()
        {
            IEnumerable<Expression<Func<Post, bool>>> createdPredicates = new List<Expression<Func<Post, bool>>>();

            PostRepositoryMock
                .Setup(m => m.GetAsync(It.IsAny<IEnumerable<Expression<Func<Post, bool>>>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Callback((IEnumerable<Expression<Func<Post, bool>>> predicates, int _, int __, CancellationToken ___) => createdPredicates = predicates);

            var result = await Mediator.Send(new SearchPostsQuery { Page = 1, SearchQuery = "text text2", ItemsPerPage = 100 });

            result.IsSuccess.Should().BeTrue();

            var predicate = createdPredicates.Where(p =>
                p.Body.ToString().Contains("p.Title.Contains(\"text\")")
                && p.Body.ToString().Contains("p.Title.Contains(\"text2\")")
                && p.Body.ToString().Contains("OrElse p.Description.Contains(\"text\")")
                && p.Body.ToString().Contains("OrElse p.Description.Contains(\"text2\")")
                && p.Body.ToString().Contains("OrElse p.Categories.Contains(\"text\")")
                && p.Body.ToString().Contains("OrElse p.Categories.Contains(\"text2\")")
                && p.Body.ToString().Contains("OrElse p.Content.Contains(\"text\")")
                && p.Body.ToString().Contains("OrElse p.Content.Contains(\"text2\")"));

            predicate.Should().NotBeNull();
        }

        [Fact]
        public async Task Handler_Should_Predicates_SearchExpression_WhenCountPosts()
        {
            IEnumerable<Expression<Func<Post, bool>>> createdPredicates = new List<Expression<Func<Post, bool>>>();

            PostRepositoryMock
                .Setup(m => m.CountAsync(It.IsAny<IEnumerable<Expression<Func<Post, bool>>>>(), It.IsAny<CancellationToken>()))
                .Callback((IEnumerable<Expression<Func<Post, bool>>> predicates, CancellationToken _) => createdPredicates = predicates);

            var result = await Mediator.Send(new SearchPostsQuery { Page = 1, SearchQuery = "text text2", ItemsPerPage = 100 });

            result.IsSuccess.Should().BeTrue();

            var predicate = createdPredicates.Where(p =>
                p.Body.ToString().Contains("p.Title.Contains(\"text\")")
                && p.Body.ToString().Contains("p.Title.Contains(\"text2\")")
                && p.Body.ToString().Contains("OrElse p.Description.Contains(\"text\")")
                && p.Body.ToString().Contains("OrElse p.Description.Contains(\"text2\")")
                && p.Body.ToString().Contains("OrElse p.Categories.Contains(\"text\")")
                && p.Body.ToString().Contains("OrElse p.Categories.Contains(\"text2\")")
                && p.Body.ToString().Contains("OrElse p.Content.Contains(\"text\")")
                && p.Body.ToString().Contains("OrElse p.Content.Contains(\"text2\")"));

            predicate.Should().NotBeNull();
        }

        private Post CreatePost(int i, Author author, bool cover)
        {
            var post = new Post
            {
                Author = author,
                Title = "title" + i,
                Slug = "slug" + i,
                Categories = "categories",
                Description = "description",
                Content = "content with a url: %URL%/content-url",
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
