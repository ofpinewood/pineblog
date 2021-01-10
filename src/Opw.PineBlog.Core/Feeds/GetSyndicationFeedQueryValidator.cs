using FluentValidation;
using Opw.PineBlog.FluentValidation;

namespace Opw.PineBlog.Feeds
{
    /// <summary>
    /// Validator for the GetSyndicationFeedQuery request.
    /// </summary>
    public class GetSyndicationFeedQueryValidator : AbstractValidator<GetSyndicationFeedQuery>
    {
        /// <summary>
        /// Implementation of GetSyndicationFeedQueryValidator.
        /// </summary>
        public GetSyndicationFeedQueryValidator()
        {
            RuleFor(c => c.FeedType).IsRequiredEnum();
            RuleFor(c => c.BaseUri).NotEmpty();
            RuleFor(c => c.PostBasePath).NotEmpty();
        }
    }
}
