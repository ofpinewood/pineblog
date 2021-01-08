using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Opw.PineBlog.Blogs;

namespace Opw.PineBlog.RazorPages.Areas.Admin.Pages
{
    public class UpdateBlogSettingsModel : PageModelBase<UpdateBlogSettingsModel>
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public UpdateBlogSettingsCommand BlogSettings { get; set; }

        public UpdateBlogSettingsModel(IMediator mediator, ILogger<UpdateBlogSettingsModel> logger)
            : base(logger)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetBlogSettingsQuery(), cancellationToken);
            if (!result.IsSuccess)
                throw result.Exception;

            BlogSettings = new UpdateBlogSettingsCommand
            {
                Title = result.Value.Title,
                Description = result.Value.Description,
                CoverUrl = result.Value.CoverUrl,
                CoverCaption = result.Value.CoverCaption,
                CoverLink = result.Value.CoverLink
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _mediator.Send(BlogSettings, cancellationToken);
            if (!result.IsSuccess)
            {
                Logger.LogError(result.Exception, "Could not update blog settings.");
                ModelState.AddModelError("", result.Exception.Message);
            }

            return Page();
        }
    }
}