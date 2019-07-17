using FluentValidation;
using MediatR;
using MediatR.Registration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Opw.PineBlog.Posts;
using Opw.MediatR;

namespace Opw.PineBlog
{
    /// <summary>
    /// Provides extension methods for the Microsoft.Extensions.DependencyInjection.IServiceCollection interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds PineBlog core services to the specified services collection.
        /// </summary>
        /// <param name="services">The services available in the application.</param>
        /// <param name="configuration">The application configuration properties.</param>
        /// <returns>The original services object.</returns>
        public static IServiceCollection AddPineBlogCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BlogOptions>(configuration.GetSection(nameof(BlogOptions)));

            // TODO: only add MediatR if it has not been added yet
            services.AddMediatR(typeof(AddPostCommand).Assembly);
            ServiceRegistrar.AddMediatRClasses(services, new[] { typeof(AddPostCommand).Assembly });

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddTransient<IValidator<AddPostCommand>, AddPostCommandValidator>();
            services.AddTransient<IValidator<GetPostQuery>, GetPostQueryValidator>();
            //services.AddTransient<IValidator<GetPostByIdQuery>, GetPostByIdQueryValidator>();

            return services;
        }
    }
}
