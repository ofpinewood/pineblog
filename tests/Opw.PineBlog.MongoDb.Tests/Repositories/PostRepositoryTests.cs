using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Opw.PineBlog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.MongoDb.Repositories
{
    public class PostRepositoryTests : MongoDbTestsBase
    {
        private Guid _authorId;
        private readonly PostRepository _postRepository;
        private readonly IBlogUnitOfWork _uow;

        public PostRepositoryTests()
        {
            SeedDatabase();

            _uow = ServiceProvider.GetService<IBlogUnitOfWork>();
            _postRepository = (PostRepository)_uow.Posts;
        }

        [Fact(Skip = Constants.SkipMongoDbTests)]
        public async Task SingleOrDefaultAsync_Should_ReturnPost()
        {
            var result = await _postRepository.SingleOrDefaultAsync(p => p.Title.Equals("Post title 0"), CancellationToken.None);

            result.Should().NotBeNull();
            result.Title.Should().Be("Post title 0");
            result.Author.Should().NotBeNull();
            result.Author.UserName.Should().Be("user@example.com");
        }

        [Fact(Skip = Constants.SkipMongoDbTests)]
        public async Task SingleOrDefaultAsync_Should_ReturnNull()
        {
            var result = await _postRepository.SingleOrDefaultAsync(p => p.Title.Equals("Post title X"), CancellationToken.None);

            result.Should().BeNull();
        }

        [Fact(Skip = Constants.SkipMongoDbTests)]
        public async Task GetNextAsync_Should_ReturnNextPost()
        {
            var result = await _postRepository.GetNextAsync(DateTime.UtcNow.AddDays(-3), CancellationToken.None);

            result.Should().NotBeNull();
            result.Title.Should().Be("Post title 2");
            result.Author.Should().BeNull();
        }

        [Fact(Skip = Constants.SkipMongoDbTests)]
        public async Task GetPreviousAsync_Should_ReturnPreviousPost()
        {
            var result = await _postRepository.GetPreviousAsync(DateTime.UtcNow.AddDays(-3), CancellationToken.None);

            result.Should().NotBeNull();
            result.Title.Should().Be("Post title 3");
            result.Author.Should().BeNull();
        }

        [Fact(Skip = Constants.SkipMongoDbTests)]
        public async Task GetPublishedAsync_Should_Return5PostsWithAuthors()
        {
            var results = await _postRepository.GetPublishedAsync(int.MaxValue, CancellationToken.None);

            results.Should().HaveCount(5);
            results.First().Author.Should().NotBeNull();
            results.Last().Author.Should().NotBeNull();
        }

        [Fact(Skip = Constants.SkipMongoDbTests)]
        public async Task CountAsync_Should_Return5()
        {
            var predicates = new List<Expression<Func<Post, bool>>>();
            predicates.Add(p => p.Published != null);

            var result = await _postRepository.CountAsync(predicates, CancellationToken.None);

            result.Should().Be(5);
        }

        [Fact(Skip = Constants.SkipMongoDbTests)]
        public async Task GetAsync_Should_Return5PostsInDescendingOrderOfPublished()
        {
            var predicates = new List<Expression<Func<Post, bool>>>();
            predicates.Add(p => p.Published != null);

            var results = await _postRepository.GetAsync(predicates, 0, int.MaxValue, CancellationToken.None);

            results.Should().HaveCount(5);
            results.Select(p => p.Published).Should().BeInDescendingOrder();
        }

        [Fact(Skip = Constants.SkipMongoDbTests)]
        public async Task GetAsync_Should_Return5PostsWithTakeAndSkip()
        {
            var predicates = new List<Expression<Func<Post, bool>>>();
            predicates.Add(p => p.Published != null);

            var results = await _postRepository.GetAsync(predicates, 1, 2, CancellationToken.None);

            results.Should().HaveCount(2);
            results.First().Title.Should().Be("Post title 1");
            results.Last().Title.Should().Be("Post title 2");
        }

        [Fact(Skip = Constants.SkipMongoDbTests)]
        public async Task Add_Should_AddPost()
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

            var result = PostCollection.Find(p => p.Title.Equals("Post title 999")).SingleOrDefault();

            result.Should().NotBeNull();
            result.Title.Should().Be("Post title 999");
            result.Description.Should().Be("Description");
        }

        [Fact(Skip = Constants.SkipMongoDbTests)]
        public async Task Update_Should_UpdatePost()
        {
            var post = PostCollection.Find(p => p.Title.Equals("Post title 1")).SingleOrDefault();
            post.Should().NotBeNull();
            post.Title.Should().Be("Post title 1");

            post.Title = "UPDATED";

            _postRepository.Update(post);
            await _uow.SaveChangesAsync(CancellationToken.None);

            var result = PostCollection.Find(p => p.Title.Equals("UPDATED")).SingleOrDefault();

            result.Should().NotBeNull();
            result.Title.Should().Be("UPDATED");
            result.Description.Should().Be("Description");
        }

        [Fact(Skip = Constants.SkipMongoDbTests)]
        public async Task Remove_Should_RemovePost()
        {
            var beforeCount = PostCollection.Find(p => true).CountDocuments();

            var post = PostCollection.Find(p => p.Title.Equals("Post title 1")).SingleOrDefault();
            post.Should().NotBeNull();
            post.Title.Should().Be("Post title 1");

            _postRepository.Remove(post);
            await _uow.SaveChangesAsync(CancellationToken.None);

            var resultCount = PostCollection.Find(p => true).CountDocuments();

            resultCount.Should().Be(beforeCount - 1);
        }

        private void SeedDatabase()
        {
            _authorId = Guid.NewGuid();
            AuthorCollection.InsertOne(new Author
            {
                Id = _authorId,
                DisplayName = "Author 1",
                UserName = "user@example.com"
            });

            PostCollection.InsertOne(CreatePost(0, _authorId, true, false, "cat1"));
            PostCollection.InsertOne(CreatePost(1, _authorId, true, true, "cat1"));
            PostCollection.InsertOne(CreatePost(2, _authorId, true, true, "cat1,cat2"));
            PostCollection.InsertOne(CreatePost(3, _authorId, true, true, "cat2"));
            PostCollection.InsertOne(CreatePost(4, _authorId, true, true, "cat1,cat2,cat3"));
            PostCollection.InsertOne(CreatePost(5, _authorId, false, true, "cat3"));
        }

        private Post CreatePost(int i, Guid authorId, bool published, bool cover, string categories)
        {
            var post = new Post
            {
                Id = Guid.NewGuid(),
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
