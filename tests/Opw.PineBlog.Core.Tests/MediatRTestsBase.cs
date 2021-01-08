using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Opw.PineBlog.Posts;
using Opw.PineBlog.Repositories;
using System;
using System.Threading;

namespace Opw.PineBlog
{
    public abstract class MediatRTestsBase
    {
        protected readonly IServiceCollection Services;

        protected IMediator Mediator => ServiceProvider.GetRequiredService<IMediator>();

        protected IServiceProvider ServiceProvider => Services.BuildServiceProvider();

        protected Mock<IBlogSettingsRepository> BlogSettingsRepositoryMock { get; }

        protected Mock<IAuthorRepository> AuthorRepositoryMock { get; }

        protected Mock<IPostRepository> PostRepositoryMock { get; }

        protected Mock<IBlogUnitOfWork> BlogUnitOfWorkMock { get; }

        public MediatRTestsBase()
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build();

            Services = new ServiceCollection();
            Services.AddMediatR(typeof(AddPostCommand).Assembly);
            Services.AddPineBlogCore(configuration);
            
            BlogSettingsRepositoryMock = new Mock<IBlogSettingsRepository>();
            AuthorRepositoryMock = new Mock<IAuthorRepository>();
            PostRepositoryMock = new Mock<IPostRepository>();

            BlogUnitOfWorkMock = new Mock<IBlogUnitOfWork>();
            BlogUnitOfWorkMock.SetupGet(m => m.BlogSettings).Returns(BlogSettingsRepositoryMock.Object);
            BlogUnitOfWorkMock.SetupGet(m => m.Authors).Returns(AuthorRepositoryMock.Object);
            BlogUnitOfWorkMock.SetupGet(m => m.Posts).Returns(PostRepositoryMock.Object);

            BlogUnitOfWorkMock.Setup(m => m.SaveChanges()).Returns(Result<int>.Success(1));
            BlogUnitOfWorkMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(Result<int>.Success(1));

            Services.AddTransient((_) => BlogUnitOfWorkMock.Object);
        }
    }
}
