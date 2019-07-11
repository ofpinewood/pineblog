using FluentValidation.Results;
using Opw.HttpExceptions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.Posts
{
    public class AddPostCommandTests : MediatRTestsBase
    {
        [Fact]
        public async Task Validator_Should_ThrowValidationErrorException()
        {
            Task action() => Mediator.Send(new AddPostCommand
            {
                AuthorId = ShortGuid.NewGuid()
            });

            await Assert.ThrowsAsync<ValidationErrorException<ValidationFailure>>(action);
        }
    }
}
