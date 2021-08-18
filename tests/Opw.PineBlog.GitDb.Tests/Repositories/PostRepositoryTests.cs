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

namespace Opw.PineBlog.GitDb.Repositories
{
    public class PostRepositoryTests : GitDbTestsBase
    {
        private readonly PostRepository _postRepository;
        private readonly IBlogUnitOfWork _uow;

        private readonly DateTime _firstPostPublishedDate = DateTime.Parse("2021-08-12T01:00:14+00:00");

        public PostRepositoryTests(GitDbFixture fixture) : base(fixture)
        {
            _uow = ServiceProvider.GetRequiredService<IBlogUnitOfWork>();
            _postRepository = (PostRepository)_uow.Posts;
        }

        [Fact]
        public async Task SingleOrDefaultAsync_Should_ReturnPost()
        {
            var result = await _postRepository.SingleOrDefaultAsync(p => p.Title.Equals("title aaa title0"), CancellationToken.None);

            result.Should().NotBeNull();
            result.Title.Should().Be("title aaa title0");
            result.Slug.Should().Be("title-aaa-0");
            result.Description.Should().Be("description aaa description0");
            result.Categories.Should().Be("categories,cata");
            result.Published.Should().Be(_firstPostPublishedDate);

            result.Author.Should().NotBeNull();
            result.Author.UserName.Should().Be("john.smith@example.com");
        }

        [Fact]
        public async Task SingleOrDefaultAsync_Should_ReturnNull()
        {
            var result = await _postRepository.SingleOrDefaultAsync(p => p.Title.Equals("Invalid Post"), CancellationToken.None);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetNextAsync_Should_ReturnNextPost()
        {
            var result = await _postRepository.GetNextAsync(_firstPostPublishedDate.AddDays(-1), CancellationToken.None);

            result.Should().NotBeNull();
            result.Title.Should().Be("title aaa title0");
            result.Author.Should().BeNull();
        }

        [Fact]
        public async Task GetPreviousAsync_Should_ReturnPreviousPost()
        {
            var result = await _postRepository.GetPreviousAsync(_firstPostPublishedDate, CancellationToken.None);

            result.Should().NotBeNull();
            result.Title.Should().Be("title aaa title1");
            result.Author.Should().BeNull();
        }

        [Fact]
        public async Task CountAsync_Should_Return6()
        {
            var predicates = new List<Expression<Func<Post, bool>>>();
            predicates.Add(p => p.Published != null);

            var result = await _postRepository.CountAsync(predicates, CancellationToken.None);

            result.Should().Be(6);
        }

        [Fact]
        public async Task GetAsync_Should_Return6PostsInDescendingOrderOfPublished()
        {
            var predicates = new List<Expression<Func<Post, bool>>>();
            predicates.Add(p => p.Published != null);

            var results = await _postRepository.GetAsync(predicates, 0, int.MaxValue, CancellationToken.None);

            results.Should().HaveCount(6);
            results.Select(p => p.Published).Should().BeInDescendingOrder();
        }

        [Fact]
        public async Task GetAsync_Should_Return1PostsWithTakeAndSkip()
        {
            var predicates = new List<Expression<Func<Post, bool>>>();
            predicates.Add(p => p.Published != null);

            var results = await _postRepository.GetAsync(predicates, 1, 1, CancellationToken.None);

            results.Should().HaveCount(1);
            results.First().Title.Should().Be("title aaa title1");
        }

        [Fact]
        public void Add_Should_ThrowNotImplementedException()
        {
            Action act = () => _postRepository.Add(new Post());

            act.Should().Throw<NotImplementedException>();
        }

        [Fact]
        public void Update_Should_ThrowNotImplementedException()
        {
            Action act = () => _postRepository.Add(new Post());

            act.Should().Throw<NotImplementedException>();
        }

        [Fact]
        public void Remove_Should_ThrowNotImplementedException()
        {
            Action act = () => _postRepository.Add(new Post());

            act.Should().Throw<NotImplementedException>();
        }
    }
}
