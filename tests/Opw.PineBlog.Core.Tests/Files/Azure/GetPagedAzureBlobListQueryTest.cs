using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Opw.PineBlog.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.Files.Azure
{
    public class GetPagedAzureBlobListQueryTest : MediatRTestsBase
    {
        public GetPagedAzureBlobListQueryTest()
        {
            // use the actual UploadAzureBlobCommand for these tests, not the mock
            Services.AddTransient<IRequestHandler<UploadAzureBlobCommand, Result<string>>, UploadAzureBlobCommand.Handler>();
        }

        [Fact(Skip = "Integration Test; requires Azure Storage Emulator.")]
        public async Task Handler_Should_ReturnFirstPageWith5Files()
        {
            var directory = $"{DateTime.UtcNow.Ticks}-{Guid.NewGuid()}";
            for (var i = 0; i < 9; i++)
            {
                var fileName = $"file-{i}.txt";
                await Mediator.Send(new UploadAzureBlobCommand { FileStream = GetFileStream(), FileName = fileName, TargetPath = directory });
            }

            var result = await Mediator.Send(new GetPagedAzureBlobListQuery { DirectoryPath = directory, Pager = new Pager(1, 5) });

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Files.Should().HaveCount(5);
            result.Value.Pager.Total.Should().Be(9);
        }

        [Fact(Skip = "Integration Test; requires Azure Storage Emulator.")]
        public async Task Handler_Should_ReturnSecondPageWith4Files()
        {
            var directory = $"{DateTime.UtcNow.Ticks}-{Guid.NewGuid()}";
            for (var i = 0; i < 9; i++)
            {
                var fileName = $"file-{i}.txt";
                await Mediator.Send(new UploadAzureBlobCommand { FileStream = GetFileStream(), FileName = fileName, TargetPath = directory });
            }

            var result = await Mediator.Send(new GetPagedAzureBlobListQuery { DirectoryPath = directory, Pager = new Pager(2, 5) });

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Files.Should().HaveCount(4);
            result.Value.Pager.Total.Should().Be(9);
        }

        public Stream GetFileStream()
        {
            var fileStream = new MemoryStream();
            var writer = new StreamWriter(fileStream);
            writer.Write("Contents of the text file.");
            writer.Flush();
            fileStream.Position = 0;
            return fileStream;

        }
    }
}
