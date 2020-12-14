using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Opw.PineBlog.Posts;
using Opw.PineBlog.Repositories;
using System;

namespace Opw.PineBlog
{
    public abstract class MediatRTestsBase
    {
        protected readonly IServiceCollection Services;

        protected IMediator Mediator => ServiceProvider.GetRequiredService<IMediator>();

        protected IServiceProvider ServiceProvider => Services.BuildServiceProvider();

        protected Mock<IBlogSettingsRepository> BlogSettingsRepositoryMock { get; }

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

            BlogUnitOfWorkMock = new Mock<IBlogUnitOfWork>();
            BlogUnitOfWorkMock.SetupGet(m => m.BlogSettings).Returns(BlogSettingsRepositoryMock.Object);
        }

        protected void AddBlogUnitOfWorkMock()
        {
            Services.AddTransient((_) => BlogUnitOfWorkMock.Object);
        }
    }
}
