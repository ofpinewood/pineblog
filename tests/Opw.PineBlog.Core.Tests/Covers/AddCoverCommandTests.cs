using FluentAssertions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Opw.HttpExceptions;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.Covers
{
    public class AddCoverCommandTests : MediatRTestsBase
    {
        private readonly Mock<IFormFile> _formFileMock;

        public AddCoverCommandTests()
        {
            var fileStream = new MemoryStream();
            var writer = new StreamWriter(fileStream);
            writer.Write("Contents of the image file.");
            writer.Flush();
            fileStream.Position = 0;

            _formFileMock = new Mock<IFormFile>();
            _formFileMock.Setup(f => f.Name).Returns("CoverImage");
            _formFileMock.Setup(f => f.FileName).Returns("image.jpg");
            _formFileMock.Setup(f => f.Length).Returns(fileStream.Length);
            _formFileMock.Setup(f => f.OpenReadStream()).Returns(fileStream);
        }

        [Fact]
        public async Task Validator_Should_ThrowValidationErrorException()
        {
            Task action() => Mediator.Send(new AddCoverCommand());

            await Assert.ThrowsAsync<ValidationErrorException<ValidationFailure>>(action);
        }

        [Fact]
        public async Task Handler_Should_AddCover()
        {
            var result = await Mediator.Send(new AddCoverCommand
            {
                File = _formFileMock.Object,
                Caption = "Cover caption",
                Link = "http://www.example.com/cover-link"
            });

            result.IsSuccess.Should().BeTrue();
            result.Value.Id.Should().NotBeEmpty();

            var context = ServiceProvider.GetRequiredService<IBlogEntityDbContext>();

            var cover = await context.Covers.SingleAsync(p => p.Caption.Equals("Cover caption"));

            cover.Should().NotBeNull();
            cover.Id.Should().Be(result.Value.Id);
            cover.Url.Should().EndWith("covers/image.jpg");
        }

        [Fact]
        public async Task Handler_Should_AddCover_WithFileNameSet()
        {
            var result = await Mediator.Send(new AddCoverCommand
            {
                File = _formFileMock.Object,
                FileName = "filename.jpg",
                Caption = "Cover caption",
                Link = "http://www.example.com/cover-link"
            });

            result.IsSuccess.Should().BeTrue();
            result.Value.Id.Should().NotBeEmpty();

            var context = ServiceProvider.GetRequiredService<IBlogEntityDbContext>();

            var cover = await context.Covers.SingleAsync(p => p.Caption.Equals("Cover caption"));

            cover.Should().NotBeNull();
            cover.Id.Should().Be(result.Value.Id);
            cover.Url.Should().EndWith("covers/filename.jpg");
        }
    }
}
