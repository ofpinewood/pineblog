using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Opw.PineBlog.Blog;

namespace Opw.PineBlog
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseBlog(this IApplicationBuilder app)
        {
            var mediator = app.ApplicationServices.GetRequiredService<IMediator>();
            mediator.Send(new ConfigureBlogOptionsCommand());

            return app;
        }
    }
}
