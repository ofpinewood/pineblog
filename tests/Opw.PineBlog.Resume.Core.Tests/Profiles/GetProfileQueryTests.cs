using FluentAssertions;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Opw.HttpExceptions;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using Opw.PineBlog.Resume.Entities;

namespace Opw.PineBlog.Resume.Profiles
{
    public class GetProfileQueryTests : MediatRTestsBase
    {
        public GetProfileQueryTests()
        {
            SeedDatabase();
        }

        [Fact]
        public async Task Validator_Should_ThrowValidationErrorException()
        {
            Task action() => Mediator.Send(new GetProfileQuery
            {
                Slug = "this is not a valid slug"
            });

            var ex = await Assert.ThrowsAsync<ValidationErrorException<ValidationFailure>>(action);
            ex.Errors.Single(e => e.Key.Equals(nameof(GetProfileQuery.Slug))).Should().NotBeNull();
        }

        [Fact]
        public async Task Handler_Should_ReturnNotFoundException()
        {
            var result = await Mediator.Send(new GetProfileQuery { Slug = "profile-not-found" });

            result.IsSuccess.Should().BeFalse();
            result.Exception.Should().BeOfType<NotFoundException<Profile>>();
        }

        [Fact]
        public async Task Handler_Should_ReturnProfile()
        {
            var result = await Mediator.Send(new GetProfileQuery { Slug = "john-smith" });

            result.IsSuccess.Should().BeTrue();
            result.Value.Headline.Should().Be("John Smith's profile");
        }

        private void SeedDatabase()
        {
            var context = ServiceProvider.GetRequiredService<IResumeEntityDbContext>();

            var profile = new Profile { UserName = "john-smith@example.com", Headline = "John Smith's profile", Slug = "john-smith" };
            context.Profiles.Add(profile);
            context.SaveChanges();
        }
    }
}
