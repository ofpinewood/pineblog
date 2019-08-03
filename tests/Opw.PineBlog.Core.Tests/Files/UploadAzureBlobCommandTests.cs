using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Opw.HttpExceptions;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.Files
{
    public class UploadAzureBlobCommandTests : MediatRTestsBase
    {
        private readonly MemoryStream _fileStream;

        public UploadAzureBlobCommandTests()
        {
            _fileStream = new MemoryStream();
            var writer = new StreamWriter(_fileStream);
            writer.Write("Contents of the text file.");
            writer.Flush();
            _fileStream.Position = 0;

            Services.AddTransient<IRequestHandler<UploadAzureBlobCommand, Result<string>>, UploadAzureBlobCommand.Handler>();
        }

        [Fact]
        public async Task Validator_Should_ThrowValidationErrorException()
        {
            Task action() => Mediator.Send(new UploadAzureBlobCommand());

            var ex = await Assert.ThrowsAsync<ValidationErrorException<ValidationFailure>>(action);
            ex.Errors.Single(e => e.Key.Equals(nameof(UploadAzureBlobCommand.FileName))).Should().NotBeNull();
            ex.Errors.Single(e => e.Key.Equals(nameof(UploadAzureBlobCommand.FileStream))).Should().NotBeNull();
        }

        [Fact(Skip = "Integration Test; requires Azure Storage Emulator.")]
        public async Task Handler_Should_ReturnTrue()
        {
            var result = await Mediator.Send(new UploadAzureBlobCommand { FileStream = _fileStream, FileName = "filename.txt", TargetPath = "files" });

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().EndWith("files/filename.txt");
        }
    }
}
