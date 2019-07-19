using FluentAssertions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Moq;
using Opw.HttpExceptions;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.Files
{
    public class UploadFileCommandTests : MediatRTestsBase
    {
        private readonly Mock<IFormFile> _formFileMock;

        public UploadFileCommandTests()
        {
            var fileStream = new MemoryStream();
            var writer = new StreamWriter(fileStream);
            writer.Write("Contents of the text file.");
            writer.Flush();
            fileStream.Position = 0;

            _formFileMock = new Mock<IFormFile>();
            _formFileMock.Setup(f => f.Name).Returns("CoverImage");
            _formFileMock.Setup(f => f.FileName).Returns("filename.txt");
            _formFileMock.Setup(f => f.Length).Returns(fileStream.Length);
            _formFileMock.Setup(f => f.OpenReadStream()).Returns(fileStream);
        }
        
        [Fact]
        public async Task Validator_Should_ThrowValidationErrorException()
        {
            Task action() => Mediator.Send(new UploadFileCommand());

            await Assert.ThrowsAsync<ValidationErrorException<ValidationFailure>>(action);
        }

        [Fact]
        public async Task Handler_Should_ReturnTrue()
        {
            var result = await Mediator.Send(new UploadFileCommand { File = _formFileMock.Object });

            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handler_Should_ReturnError_WithEmptyFile()
        {
            _formFileMock.Setup(f => f.Length).Returns(0);

            var result = await Mediator.Send(new UploadFileCommand { File = _formFileMock.Object });

            result.Exception.Should().BeOfType<FileUploadException>();
            result.Exception.Message.Should().Contain("is empty");
        }

        [Fact]
        public async Task Handler_Should_ReturnError_WithTooLargeFile()
        {
            _formFileMock.Setup(f => f.Length).Returns(2048576);

            var result = await Mediator.Send(new UploadFileCommand { File = _formFileMock.Object });

            result.Exception.Should().BeOfType<FileUploadException>();
            result.Exception.Message.Should().Contain("exceeds");
        }

        [Fact]
        public async Task Handler_Should_ReturnError_WhenFileNull()
        {
            _formFileMock.Setup(f => f.OpenReadStream()).Returns<Stream>(null);

            var result = await Mediator.Send(new UploadFileCommand { File = _formFileMock.Object });

            result.Exception.Should().BeOfType<FileUploadException>();
            result.Exception.Message.Should().Contain("upload failed");
        }
    }
}
