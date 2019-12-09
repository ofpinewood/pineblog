using FluentValidation;
using MediatR;
using MediatR.Registration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Opw.MediatR;
using Opw.PineBlog.Resume.Profiles;

namespace Opw.PineBlog.Resume
{
    /// <summary>
    /// Provides extension methods for the Microsoft.Extensions.DependencyInjection.IServiceCollection interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds PineBlog:Resume core services to the specified services collection.
        /// </summary>
        /// <param name="services">The services available in the application.</param>
        /// <param name="configuration">The application configuration properties.</param>
        /// <returns>The original services object.</returns>
        public static IServiceCollection AddPineBlogResumeCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<PineBlogOptions>(configuration.GetSection(nameof(PineBlogOptions)));

            if (services.BuildServiceProvider().GetService<IMediator>() == null)
                services.AddMediatR(typeof(GetProfileQuery).Assembly);
            ServiceRegistrar.AddMediatRClasses(services, new[] { typeof(GetProfileQuery).Assembly });

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            services.AddTransient<IValidator<GetProfileQuery>, GetProfileQueryValidator>();
            services.AddTransient<IValidator<GetProfileByUserQuery>, GetProfileByUserQueryValidator>();

            return services;
        }
    }
}
