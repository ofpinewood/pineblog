using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Opw.PineBlog.Covers;

namespace Opw.PineBlog.Areas.Admin.Pages
{
    public class AddCoverModel : PageModelBase<AddCoverModel>
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public AddCoverCommand Cover { get; set; }

        public AddCoverModel(IMediator mediator, ILogger<AddCoverModel> logger) : base(logger)
        {
            _mediator = mediator;
        }

        public IActionResult OnGet()
        {
            Cover = new AddCoverCommand();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _mediator.Send(Cover, cancellationToken);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return Page();
            }

            return RedirectToPage("Covers");
        }
    }
}