using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Opw.PineBlog
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseBlog(this IApplicationBuilder app)
        {
            // TODO: fix
            //var mediator = app.ApplicationServices.GetRequiredService<IMediator>();
            //mediator.Send(new ConfigureBlogOptionsCommand());

            return app;
        }
    }
}
