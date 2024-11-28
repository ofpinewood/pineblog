using FluentValidation;
using MediatR;
using MediatR.Registration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Opw.PineBlog.Posts;
using Opw.PineBlog.MediatR;
using Opw.PineBlog.Files;
using Microsoft.Extensions.Options;
using Opw.PineBlog.Files.AzureBlobs;
using Opw.PineBlog.Feeds;
using Opw.PineBlog.Blogs;
using Opw.PineBlog.Posts.Search;
using System.Collections.Generic;
using Opw.PineBlog.FeatureManagement;
using System;
using Azure.Storage.Blobs;

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
        /// <remarks>This only adds the PineBlog core services not that data layer.
        /// To add the data layer use one of the specific methods for that, e.g. AddPineBlogEntityFrameworkCore.</remarks>
        /// <returns>The original services object.</returns>
        public static IServiceCollection AddPineBlogCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<PineBlogOptions>(configuration.GetSection(nameof(PineBlogOptions)));

            if (services.BuildServiceProvider().GetService<IMediator>() == null)
            {
                var mediatRServiceConfiguration = new MediatRServiceConfiguration();
                mediatRServiceConfiguration.RegisterServicesFromAssembly(typeof(AddPostCommand).Assembly);

                services.AddMediatR(mediatRServiceConfiguration);
                ServiceRegistrar.AddMediatRClasses(services, mediatRServiceConfiguration);
            }

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            services.AddTransient<IValidator<AddPostCommand>, AddPostCommandValidator>();
            services.AddTransient<IValidator<UpdatePostCommand>, UpdatePostCommandValidator>();
            services.AddTransient<IValidator<PublishPostCommand>, PublishPostCommandValidator>();
            services.AddTransient<IValidator<UnpublishPostCommand>, UnpublishPostCommandValidator>();
            services.AddTransient<IValidator<DeletePostCommand>, DeletePostCommandValidator>();

            services.AddTransient<IValidator<GetPostQuery>, GetPostQueryValidator>();
            services.AddTransient<IValidator<GetPostByIdQuery>, GetPostByIdQueryValidator>();

            services.AddTransient<IValidator<GetSyndicationFeedQuery>, GetSyndicationFeedQueryValidator>();

            services.AddTransient<IValidator<UpdateBlogSettingsCommand>, UpdateBlogSettingsCommandValidator>();

            services.AddTransient<IPostRanker, PostRanker>();

            services.AddTransient<IUploadFileCommandFactory, UploadFileCommandFactory>();
            services.AddTransient<IDeleteFileCommandFactory, DeleteFileCommandFactory>();
            services.AddTransient<IGetPagedFileListQueryFactory, GetPagedFileListQueryFactory>();

            services.AddTransient<FileUrlHelper>();
            services.AddTransient<PostUrlHelper>();
            services.AddTransient<AzureBlobHelper>();

            services.AddPineBlogCoreAzureServices();
            services.AddFeatureManagement();

            return services;
        }

        private static IServiceCollection AddPineBlogCoreAzureServices(this IServiceCollection services)
        {
            services.AddSingleton<BlobServiceClient>((provider) =>
            {
                var options = provider.GetRequiredService<IOptions<PineBlogOptions>>();

                try
                {
                    return new BlobServiceClient(options.Value.AzureStorageConnectionString);
                }
                catch
                {
                    throw new ConfigurationException("The PineBlogOptions.AzureStorageConnectionString is invalid.");
                }
            });

            services.AddTransient<IValidator<UploadAzureBlobCommand>, UploadAzureBlobCommandValidator>();
            services.AddTransient<IValidator<DeleteAzureBlobCommand>, DeleteAzureBlobCommandValidator>();

            return services;
        }

        private static IServiceCollection AddFeatureManagement(this IServiceCollection services)
        {
            var features = new Dictionary<FeatureFlag, FeatureState>();
            foreach (FeatureFlag featureFlag in Enum.GetValues(typeof(FeatureFlag)))
            {
                features.Add(featureFlag, FeatureState.Enabled());
            }

            services.AddScoped<IFeatureManager>((_) => new FeatureManager(features));

            return services;
        }
    }
}
