using FluentAssertions;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Opw.HttpExceptions;
using Opw.PineBlog.Resume.Entities;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.Resume.Profiles
{
    public class AddOrUpdateProfileCommandTests : MediatRTestsBase
    {
        public AddOrUpdateProfileCommandTests()
        {
            SeedDatabase();
        }

        [Fact]
        public async Task Validator_Should_ThrowValidationErrorException()
        {
            Task action() => Mediator.Send(new AddOrUpdateProfileCommand());

            var ex = await Assert.ThrowsAsync<ValidationErrorException<ValidationFailure>>(action);
            ex.Errors.Single(e => e.Key.Equals(nameof(AddOrUpdateProfileCommand.UserName))).Should().NotBeNull();
        }

        [Fact]
        public async Task Handler_Should_AddProfile()
        {
            var userName = "bart-jones@example.com";
            var result = await Mediator.Send(new AddOrUpdateProfileCommand
            {
                UserName = userName,
                Headline = "headline"
            });

            result.IsSuccess.Should().BeTrue();
            result.Value.UserName.Should().Be(userName);

            var context = ServiceProvider.GetRequiredService<IResumeEntityDbContext>();

            var profile = await context.Profiles.SingleAsync(p => p.Headline.Equals("headline"));

            profile.Should().NotBeNull();
            profile.UserName.Should().Be(userName);
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
