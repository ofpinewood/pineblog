using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Opw.PineBlog.Areas.Admin.Pages
{
    public class IndexModel : PageModelBase<IndexModel>
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator, ILogger<IndexModel> logger) : base(logger)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            //var result = await _mediator.Send(new GetPagedPostListQuery { Page = page }, cancellationToken);

            //Model = result.Value;
            //Title = Model.Blog.Title;

            return Page();
        }
    }
}
