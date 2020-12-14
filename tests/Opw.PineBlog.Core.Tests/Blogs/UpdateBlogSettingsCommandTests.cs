//using FluentAssertions;
//using FluentValidation.Results;
//using Microsoft.Extensions.DependencyInjection;
//using Moq;
//using Opw.HttpExceptions;
//using Opw.PineBlog.Entities;
//using Opw.PineBlog.Repositories;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using Xunit;

//namespace Opw.PineBlog.Blogs
//{
//    public class UpdateBlogSettingsCommandTests : MediatRTestsBase
//    {
//        public UpdateBlogSettingsCommandTests()
//        {
//            Services.AddTransient((_) => CreateBlogUnitOfWorkMock().Object);
//        }

//        [Fact]
//        public async Task Validator_Should_ThrowValidationErrorException()
//        {
//            Task action() => Mediator.Send(new UpdateBlogSettingsCommand());

//            var ex = await Assert.ThrowsAsync<ValidationErrorException<ValidationFailure>>(action);
//            ex.Errors.Single(e => e.Key.Equals(nameof(UpdateBlogSettingsCommand.Title))).Should().NotBeNull();
//        }

//        [Fact]
//        public async Task Handler_Should_AddSettings_WhenNotFound()
//        {
//            Services.AddTransient((_) => CreateBlogUnitOfWorkMock(hasBlogSettings: false).Object);
            
//            var result = await Mediator.Send(new UpdateBlogSettingsCommand
//            {
//                Title = "blog title-NEW"
//            });

//            result.IsSuccess.Should().BeTrue();

//            context = ServiceProvider.GetRequiredService<BlogEntityDbContext>();

//            var settings = await context.BlogSettings.SingleAsync();

//            settings.Should().NotBeNull();
//            settings.Title.Should().Be("blog title-NEW");
//        }

//        [Fact]
//        public async Task Handler_Should_UpdatePost()
//        {
//            var result = await Mediator.Send(new UpdateBlogSettingsCommand
//            {
//                Title = "blog title-UPDATED",
//                Description = "blog description",
//                CoverCaption = "blog cover caption",
//                CoverLink = "blog cover link",
//                CoverUrl = "blog cover url"
//            });

//            result.IsSuccess.Should().BeTrue();

//            var context = ServiceProvider.GetRequiredService<BlogEntityDbContext>();

//            var settings = await context.BlogSettings.SingleAsync();

//            settings.Should().NotBeNull();
//            settings.Title.Should().Be("blog title-UPDATED");
//        }

//        private Mock<IBlogUnitOfWork> CreateBlogUnitOfWorkMock(bool hasBlogSettings = true)
//        {
//            var blogSettings = new List<BlogSettings>();
//            if (hasBlogSettings)
//            {
//                blogSettings.Add(new BlogSettings
//                {
//                    Title = "blog title",
//                    Description = "blog description",
//                    CoverCaption = "blog cover caption",
//                    CoverLink = "blog cover link",
//                    CoverUrl = "blog cover url"
//                });
//            }

//            var blogSettingsRepositoryMock = new Mock<IBlogSettingsRepository>();
//            blogSettingsRepositoryMock.Setup(m => m.SingleOrDefaultAsync(It.IsAny<CancellationToken>())).ReturnsAsync(blogSettings.SingleOrDefault());

//            var uowMock = new Mock<IBlogUnitOfWork>();
//            uowMock.SetupGet(m => m.BlogSettings).Returns(blogSettingsRepositoryMock.Object);

//            return uowMock;
//        }
//    }
//}
