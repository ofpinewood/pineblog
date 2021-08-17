using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.GitDb
{
    public class BlogUnitOfWorkTests : GitDbTestsBase
    {
        private readonly BlogUnitOfWork _uow;

        public BlogUnitOfWorkTests()
        {
            var options = ServiceProvider.GetRequiredService<IOptionsSnapshot<PineBlogGitDbOptions>>();
            _uow = new BlogUnitOfWork(options);
        }

        [Fact]
        public void BlogUnitOfWork_Should_HaveInitializedAllRepositories()
        {
            _uow.Should().NotBeNull();
            _uow.BlogSettings.Should().NotBeNull();
            _uow.Authors.Should().NotBeNull();
            _uow.Posts.Should().NotBeNull();
        }

        [Fact]
        public void SaveChanges_Should_ThrowNotImplementedException()
        {
            Action act = () => _uow.SaveChanges();

            act.Should().Throw<NotImplementedException>();
        }

        [Fact]
        public void SaveChangesAsync_Should_ThrowNotImplementedException()
        {
            Func<Task> act = async () => await _uow.SaveChangesAsync(CancellationToken.None);

            act.Should().Throw<NotImplementedException>();
        }
    }
}