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
    public class GetProfileByUserQueryTests : MediatRTestsBase
    {
        public GetProfileByUserQueryTests()
        {
            SeedDatabase();
        }

        [Fact]
        public async Task Validator_Should_ThrowValidationErrorException()
        {
            Task action() => Mediator.Send(new GetProfileByUserQuery());

            var ex = await Assert.ThrowsAsync<ValidationErrorException<ValidationFailure>>(action);
            ex.Errors.Single(e => e.Key.Equals(nameof(GetProfileByUserQuery.UserName))).Should().NotBeNull();
        }

        [Fact]
        public async Task Handler_Should_ReturnNotFoundException()
        {
            var result = await Mediator.Send(new GetProfileByUserQuery { UserName = "invalid@example.com" });

            result.IsSuccess.Should().BeFalse();
            result.Exception.Should().BeOfType<NotFoundException<Profile>>();
        }

        [Fact]
        public async Task Handler_Should_ReturnProfile()
        {
            var result = await Mediator.Send(new GetProfileByUserQuery { UserName = "test@example.com" });

            result.IsSuccess.Should().BeTrue();
            result.Value.Headline.Should().Be("Profile 0");
        }

        private void SeedDatabase()
        {
            var context = ServiceProvider.GetRequiredService<IResumeEntityDbContext>();

            var profile = new Profile { Headline = "Profile 0", UserName = "test@example.com" };
            context.Profiles.Add(profile);
            context.SaveChanges();
        }
    }
}
