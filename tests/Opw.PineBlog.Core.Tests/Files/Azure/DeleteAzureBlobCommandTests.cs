using FluentAssertions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Moq;
using Opw.HttpExceptions;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.Files.Azure
{
    public class DeleteAzureBlobCommandTests : MediatRTestsBase
    {
        private readonly Mock<IFormFile> _formFileMock;

        public DeleteAzureBlobCommandTests()
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
            Task action() => Mediator.Send(new DeleteAzureBlobCommand());

            var ex = await Assert.ThrowsAsync<ValidationErrorException<ValidationFailure>>(action);
            ex.Errors.Single(e => e.Key.Equals(nameof(DeleteAzureBlobCommand.FileName))).Should().NotBeNull();
        }

        [Fact(Skip = Constants.SkipAzureStorageEmulatorTests)]
        public async Task Handler_Should_ReturnTrue()
        {
            await Mediator.Send(new UploadAzureBlobCommand { File = _formFileMock.Object, TargetPath = "files", AllowedFileType = FileType.All });

            var result = await Mediator.Send(new DeleteAzureBlobCommand { FileName = "filename.txt", TargetPath = "files" });

            result.IsSuccess.Should().BeTrue();
        }

        [Fact(Skip = Constants.SkipAzureStorageEmulatorTests)]
        public async Task Handler_Should_ReturnFalse_WhenFileDoesNotExist()
        {
            var result = await Mediator.Send(new DeleteAzureBlobCommand { FileName = "invalid-filename.txt", TargetPath = "files" });

            result.IsSuccess.Should().BeFalse();
            result.Exception.Should().BeOfType<FileDeleteException>();
            result.Exception.Message.Should().Contain("does not exist");
        }
    }
}
