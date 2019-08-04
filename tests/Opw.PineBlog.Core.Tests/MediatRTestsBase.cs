using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Opw.PineBlog.EntityFrameworkCore;
using Opw.PineBlog.Files.Azure;
using Opw.PineBlog.Models;
using Opw.PineBlog.Posts;
using System;
using System.Threading;

namespace Opw.PineBlog
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

            Services = new ServiceCollection();
            Services.AddMediatR(typeof(AddPostCommand).Assembly);
            Services.AddPineBlogCore(configuration);
            Services.AddPineBlogEntityFrameworkCore($"Server=inMemory; Database=opw-db-{DateTime.UtcNow.Ticks};");

            Services.AddTransient((_) => {
                var mock = new Mock<IRequestHandler<UploadAzureBlobCommand, Result<string>>>();
                mock.Setup(h => h.Handle(It.IsAny<UploadAzureBlobCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((UploadAzureBlobCommand request, CancellationToken __) =>
                        Result<string>.Success($"http://azureblobstorage/pineblog-tests/{request.TargetPath}/{request.FileName}"));
                return mock.Object;
            });

            Services.AddTransient((_) => {
                var mock = new Mock<IRequestHandler<GetPagedAzureBlobListQuery, Result<FileListModel>>>();
                mock.Setup(h => h.Handle(It.IsAny<GetPagedAzureBlobListQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((GetPagedAzureBlobListQuery request, CancellationToken __) =>
                        Result<FileListModel>.Success(new FileListModel { Pager = request.Pager }));
                return mock.Object;
            });
        }
    }
}
