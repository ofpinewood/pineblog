using Microsoft.AspNetCore.Builder;

namespace Opw.PineBlog
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseBlog(this IApplicationBuilder app)
        {
            // TODO: set blog setting from configuration on first run
            //var mediator = app.ApplicationServices.GetRequiredService<IMediator>();
            //mediator.Send(new UpdatePineBlogOptionsCommand());

            return app;
        }
    }
}
