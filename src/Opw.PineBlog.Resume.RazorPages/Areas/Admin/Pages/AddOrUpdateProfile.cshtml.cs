using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Opw.PineBlog.Resume.Profiles;

namespace Opw.PineBlog.Resume.RazorPages.Areas.Admin.Pages
{
    public class AddOrUpdateProfileModel : PageModelBase<AddOrUpdateProfileModel>
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public AddOrUpdateProfileCommand Profile { get; set; }

        public AddOrUpdateProfileModel(IMediator mediator, ILogger<AddOrUpdateProfileModel> logger)
            : base(logger)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetProfileByUserQuery { UserName = "TODO: username" }, cancellationToken);
            if (!result.IsSuccess)
                throw result.Exception;

            Profile = new AddOrUpdateProfileCommand
            {
                UserName = result.Value.UserName,
                Slug = result.Value.Slug,
                FirstName = result.Value.FirstName,
                LastName = result.Value.LastName,
                Email = result.Value.Email,
                Headline = result.Value.Headline,
                Industry = result.Value.Industry,
                Summary = result.Value.Summary,
                Country = result.Value.Country,
                Region = result.Value.Region,
                Links = result.Value.Links,
                Experiences = result.Value.Experiences,
                Education = result.Value.Education,
                Skills = result.Value.Skills,
                Languages = result.Value.Languages
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _mediator.Send(Profile, cancellationToken);
            if (!result.IsSuccess)
            {
                Logger.LogError(result.Exception, "Could not update profile.");
                ModelState.AddModelError("", result.Exception.Message);
            }

            return Page();
        }
    }
}
