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
        protected readonly IMediator Mediator;
        protected readonly IServiceProvider ServiceProvider;

        public MediatRTestsBase()
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build();

            var services = new ServiceCollection();
            services.AddMediatR(typeof(AddPostCommand).Assembly);
            services.AddPineBlogCore(configuration);
            services.AddPineBlogEntityFrameworkCore($"Server=inMemory; Database=opw-db-{DateTime.UtcNow.Ticks};");

            ServiceProvider = services.BuildServiceProvider();

            Mediator = ServiceProvider.GetRequiredService<IMediator>();
        }
    }
}
