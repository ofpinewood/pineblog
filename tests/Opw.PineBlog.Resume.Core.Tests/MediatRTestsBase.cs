using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Opw.PineBlog.Resume.EntityFrameworkCore;
using Opw.PineBlog.Resume.Profiles;
using System;

namespace Opw.PineBlog.Resume
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
               .Build();

            // create a new in-memory database for each test
            configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value = $"Server=inMemory; Database=pineblog-tests-{DateTime.UtcNow.Ticks};";

            Services = new ServiceCollection();
            Services.AddMediatR(typeof(GetProfileQuery).Assembly);
            Services.AddPineBlogResumeCore(configuration);
            Services.AddPineBlogResumeEntityFrameworkCore(configuration);
        }
    }
}
