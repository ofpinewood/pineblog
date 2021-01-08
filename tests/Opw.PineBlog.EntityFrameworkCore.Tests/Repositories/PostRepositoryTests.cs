using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Opw.PineBlog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.EntityFrameworkCore.Repositories
{
    public class PostRepositoryTests : EntityFrameworkCoreTestsBase
    {
        private Guid _authorId;
        private readonly PostRepository _postRepository;
        private readonly IBlogUnitOfWork _uow;
        private readonly BlogEntityDbContext _dbContext;

        public PostRepositoryTests()
        {
            SeedDatabase();

            _dbContext = ServiceProvider.GetRequiredService<BlogEntityDbContext>();
            _uow = ServiceProvider.GetRequiredService<IBlogUnitOfWork>();
            _postRepository = (PostRepository)_uow.Posts;
        }

        [Fact]
        public async Task SingleOrDefaultAsync_Should_ReturnPost()
        {
            var result = await _postRepository.SingleOrDefaultAsync(p => p.Title.Equals("Post title 0"), CancellationToken.None);

            result.Should().NotBeNull();
            result.Title.Should().Be("Post title 0");
            result.Author.Should().NotBeNull();
            result.Author.UserName.Should().Be("user@example.com");
        }

        [Fact]
        public async Task SingleOrDefaultAsync_Should_ReturnNull()
        {
            var result = await _postRepository.SingleOrDefaultAsync(p => p.Title.Equals("Post title X"), CancellationToken.None);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetNextAsync_Should_ReturnNextPost()
        {
            var result = await _postRepository.GetNextAsync(DateTime.UtcNow.AddDays(-3), CancellationToken.None);

            result.Should().NotBeNull();
            result.Title.Should().Be("Post title 2");
            result.Author.Should().BeNull();
        }

        [Fact]
        public async Task GetPreviousAsync_Should_ReturnPreviousPost()
        {
            var result = await _postRepository.GetPreviousAsync(DateTime.UtcNow.AddDays(-3), CancellationToken.None);

            result.Should().NotBeNull();
            result.Title.Should().Be("Post title 3");
            result.Author.Should().BeNull();
        }

        [Fact]
        public async Task CountAsync_Should_Return5()
        {
            var predicates = new List<Expression<Func<Post, bool>>>();
            predicates.Add(p => p.Published != null);

            var result = await _postRepository.CountAsync(predicates, CancellationToken.None);

            result.Should().Be(5);
        }

        [Fact]
        public async Task GetAsync_Should_Return5PostsInDescendingOrderOfPublished()
        {
            var predicates = new List<Expression<Func<Post, bool>>>();
            predicates.Add(p => p.Published != null);

            var results = await _postRepository.GetAsync(predicates, 0, int.MaxValue, CancellationToken.None);

            results.Should().HaveCount(5);
            results.Select(p => p.Published).Should().BeInDescendingOrder();
        }

        [Fact]
        public async Task GetAsync_Should_Return5PostsWithTakeAndSkip()
        {
            var predicates = new List<Expression<Func<Post, bool>>>();
            predicates.Add(p => p.Published != null);

            var results = await _postRepository.GetAsync(predicates, 1, 2, CancellationToken.None);

            results.Should().HaveCount(2);
            results.First().Title.Should().Be("Post title 1");
            results.Last().Title.Should().Be("Post title 2");
        }

        [Fact]
        public async Task Add_Should_AddBlogSettings()
        {
            var post = new Post
            {
                AuthorId = _authorId,
                Title = "Post title 999",
                Slug = "post-title-999",
                Categories = "cat999",
                Description = "Description",
                Content = "Content"
            };

            _postRepository.Add(post);
            await _uow.SaveChangesAsync(CancellationToken.None);

            var result = _dbContext.Posts.SingleOrDefault(p => p.Title.Equals("Post title 999"));

            result.Should().NotBeNull();
            result.Title.Should().Be("Post title 999");
            result.Description.Should().Be("Description");
        }

        [Fact]
        public async Task Update_Should_UpdateBlogSettings()
        {
            var post = _dbContext.Posts.SingleOrDefault(p => p.Title.Equals("Post title 1"));
            post.Should().NotBeNull();
            post.Title.Should().Be("Post title 1");

            post.Title = "UPDATED";

            _postRepository.Update(post);
            await _uow.SaveChangesAsync(CancellationToken.None);

            var result = _dbContext.Posts.SingleOrDefault(p => p.Title.Equals("UPDATED"));

            result.Should().NotBeNull();
            result.Title.Should().Be("UPDATED");
            result.Description.Should().Be("Description");
        }

        [Fact]
        public async Task Remove_Should_UpdateBlogSettings()
        {
            var beforeCount = _dbContext.Posts.Count();

            var post = _dbContext.Posts.SingleOrDefault(p => p.Title.Equals("Post title 1"));
            post.Should().NotBeNull();
            post.Title.Should().Be("Post title 1");

            _postRepository.Remove(post);
            await _uow.SaveChangesAsync(CancellationToken.None);

            var resultCount = _dbContext.Posts.Count();

            resultCount.Should().Be(beforeCount - 1);
        }

        private void SeedDatabase()
        {
            var context = ServiceProvider.GetRequiredService<BlogEntityDbContext>();

            var author = new Author { UserName = "user@example.com", DisplayName = "Author 1" };
            context.Authors.Add(author);
            context.SaveChanges();

            _authorId = author.Id;

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

            if (published) post.Published = DateTime.UtcNow.AddDays(-i);
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
