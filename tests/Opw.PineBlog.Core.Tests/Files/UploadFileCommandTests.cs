using FluentAssertions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Moq;
using Opw.HttpExceptions;
using Opw.PineBlog.Models;
using System.IO;
using System.Linq;
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

            var ex = await Assert.ThrowsAsync<ValidationErrorException<ValidationFailure>>(action);
            ex.Errors.Single(e => e.Key.Equals(nameof(UploadFileCommand.File))).Should().NotBeNull();
            ex.Errors.Single(e => e.Key.Equals(nameof(UploadFileCommand.AllowedFileType))).Should().NotBeNull();
        }

        [Fact]
        public async Task Handler_Should_ReturnTrue()
        {
            var result = await Mediator.Send(new UploadFileCommand { File = _formFileMock.Object, TargetPath = "files", AllowedFileType = FileType.All });

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().EndWith("files/filename.txt");
        }

        [Fact]
        public async Task Handler_Should_ReturnError_WithEmptyFile()
        {
            _formFileMock.Setup(f => f.Length).Returns(0);

            Task action() => Mediator.Send(new UploadFileCommand { File = _formFileMock.Object, AllowedFileType = FileType.All });

            var ex = await Assert.ThrowsAsync<ValidationErrorException<ValidationFailure>>(action);
            ex.Errors.Single(e => e.Key.Equals(nameof(UploadFileCommand.File))).Value[0].ErrorMessage.Should().Contain("is empty");
        }

        [Fact]
        public async Task Handler_Should_ReturnError_WithTooLargeFile()
        {
            _formFileMock.Setup(f => f.Length).Returns(2048576);

            Task action() => Mediator.Send(new UploadFileCommand { File = _formFileMock.Object, AllowedFileType = FileType.All });

            var ex = await Assert.ThrowsAsync<ValidationErrorException<ValidationFailure>>(action);
            ex.Errors.Single(e => e.Key.Equals(nameof(UploadFileCommand.File))).Value[0].ErrorMessage.Should().Contain("exceeds");
        }

        [Fact]
        public async Task Handler_Should_ReturnError_WithInvalidFileType()
        {
            Task action() => Mediator.Send(new UploadFileCommand { File = _formFileMock.Object, AllowedFileType = FileType.Image });

            var ex = await Assert.ThrowsAsync<ValidationErrorException<ValidationFailure>>(action);
            ex.Errors.Single(e => e.Key.Equals(nameof(UploadFileCommand.File))).Value[0].ErrorMessage.Should().Contain("must be of type");
        }

        [Fact]
        public async Task Handler_Should_ReturnError_WhenFileNull()
        {
            _formFileMock.Setup(f => f.OpenReadStream()).Returns<Stream>(null);

            var result = await Mediator.Send(new UploadFileCommand { File = _formFileMock.Object, AllowedFileType = FileType.All });

            result.Exception.Should().BeOfType<FileUploadException>();
            result.Exception.Message.Should().Contain("upload failed");
        }
    }
}
