using FluentValidation;
using MediatR;
using MediatR.Registration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Opw.PineBlog.Posts;
using Opw.PineBlog.MediatR;
using Opw.PineBlog.Files;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Storage;
using Microsoft.Extensions.Options;
using Opw.PineBlog.Files.Azure;
using Opw.PineBlog.Feeds;
using Opw.PineBlog.Blogs;
using Opw.PineBlog.Posts.Search;
using System.Collections.Generic;
using Opw.PineBlog.FeatureManagement;
using System;

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
                services.AddMediatR(typeof(AddPostCommand).Assembly);
            ServiceRegistrar.AddMediatRClasses(services, new[] { typeof(AddPostCommand).Assembly });

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
            services.AddSingleton<CloudBlobClient>((provider) =>
            {
                var options = provider.GetRequiredService<IOptions<PineBlogOptions>>();

                CloudStorageAccount storageAccount;
                if (!CloudStorageAccount.TryParse(options.Value.AzureStorageConnectionString, out storageAccount))
                    throw new ConfigurationException("The PineBlogOptions.AzureStorageConnectionString is invalid.");

                return storageAccount.CreateCloudBlobClient();
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
