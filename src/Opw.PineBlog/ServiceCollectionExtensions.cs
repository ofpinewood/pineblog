using FluentValidation;
using MediatR;
using MediatR.Registration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Opw.PineBlog.Posts;
using Opw.MediatR;

namespace Opw.PineBlog
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBlog(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BlogOptions>(configuration.GetSection(nameof(BlogOptions)));

            // TODO: only add MediatR if it has not been added yet
            services.AddMediatR(typeof(AddPostCommand).Assembly);
            ServiceRegistrar.AddMediatRClasses(services, new[] { typeof(AddPostCommand).Assembly });

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddTransient<IValidator<AddPostCommand>, AddPostCommandValidator>();
            services.AddTransient<IValidator<GetPostQuery>, GetPostQueryValidator>();

            return services;
        }
    }
}
