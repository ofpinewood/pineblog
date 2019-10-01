using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Opw.PineBlog.EntityFrameworkCore;
using Opw.PineBlog.Posts;
using System;

namespace Opw.PineBlog
{
    public abstract class MediatRTestsBase
    {
        protected readonly IServiceCollection Services;

        protected IMediator Mediator => ServiceProvider.GetRequiredService<IMediator>();

        protected IServiceProvider ServiceProvider => Services.BuildServiceProvider();

        public MediatRTestsBase()
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .AddPineBlogConfiguration(reloadOnChange: false)
               .Build();

            // create a new in-memory database for each test
            configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value = $"Server=inMemory; Database=pineblog-tests-{DateTime.UtcNow.Ticks};";

            Services = new ServiceCollection();
            Services.AddMediatR(typeof(AddPostCommand).Assembly);
            Services.AddPineBlogCore(configuration);
            Services.AddPineBlogEntityFrameworkCore(configuration);
        }
    }
}
