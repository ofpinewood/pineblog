using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.Files.Azure
{
    public class GetPagedAzureBlobListQueryTest : MediatRTestsBase
    {
        [Fact(Skip = Constants.SkipAzureStorageEmulatorTests)]
        public async Task Handler_Should_ReturnFirstPageWith5Files()
        {
            var directory = $"{DateTime.UtcNow.Ticks}-{Guid.NewGuid()}";
            for (var i = 0; i < 9; i++)
            {
                var fileName = $"file-{i}.txt";
                await Mediator.Send(new UploadAzureBlobCommand { File = GetFormFile(fileName), TargetPath = directory, AllowedFileType = FileType.All });
            }

            var result = await Mediator.Send(new GetPagedAzureBlobListQuery { Page = 1, ItemsPerPage = 5, DirectoryPath = directory, FileType = FileType.All });

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Files.Should().HaveCount(5);
            result.Value.Pager.Total.Should().Be(9);
        }

        [Fact(Skip = Constants.SkipAzureStorageEmulatorTests)]
        public async Task Handler_Should_ReturnSecondPageWith4Files()
        {
            var directory = $"{DateTime.UtcNow.Ticks}-{Guid.NewGuid()}";
            for (var i = 0; i < 9; i++)
            {
                var fileName = $"file-{i}.txt";
                await Mediator.Send(new UploadAzureBlobCommand { File = GetFormFile(fileName), TargetPath = directory, AllowedFileType = FileType.All });
            }

            var result = await Mediator.Send(new GetPagedAzureBlobListQuery { Page = 2, ItemsPerPage = 5, DirectoryPath = directory, FileType = FileType.All });

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Files.Should().HaveCount(4);
            result.Value.Pager.Total.Should().Be(9);
        }

        [Fact(Skip = Constants.SkipAzureStorageEmulatorTests)]
        public async Task Handler_Should_Return3Files_ForFileTypeImage()
        {
            var directory = $"{DateTime.UtcNow.Ticks}-{Guid.NewGuid()}";
            await Mediator.Send(new UploadAzureBlobCommand { File = GetFormFile("file-0.txt"), TargetPath = directory, AllowedFileType = FileType.All });
            await Mediator.Send(new UploadAzureBlobCommand { File = GetFormFile("file-1.txt"), TargetPath = directory, AllowedFileType = FileType.All });
            await Mediator.Send(new UploadAzureBlobCommand { File = GetFormFile("file-2.gif"), TargetPath = directory, AllowedFileType = FileType.All });
            await Mediator.Send(new UploadAzureBlobCommand { File = GetFormFile("file-3.jpg"), TargetPath = directory, AllowedFileType = FileType.All });
            await Mediator.Send(new UploadAzureBlobCommand { File = GetFormFile("file-4.png"), TargetPath = directory, AllowedFileType = FileType.All });

            var result = await Mediator.Send(new GetPagedAzureBlobListQuery { Page = 1, ItemsPerPage = 5, DirectoryPath = directory, FileType = FileType.Image });

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Files.Should().HaveCount(3);
            result.Value.Pager.Total.Should().Be(3);
        }

        private IFormFile GetFormFile(string fileName)
        {
            var fileStream = new MemoryStream();
            var writer = new StreamWriter(fileStream);
            writer.Write("Contents of the text file.");
            writer.Flush();
            fileStream.Position = 0;

            var formFileMock = new Mock<IFormFile>();
            formFileMock.Setup(f => f.Name).Returns("CoverImage");
            formFileMock.Setup(f => f.FileName).Returns(fileName);
            formFileMock.Setup(f => f.Length).Returns(fileStream.Length);
            formFileMock.Setup(f => f.OpenReadStream()).Returns(fileStream);

            return formFileMock.Object;
        }
    }
}
