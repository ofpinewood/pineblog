using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Opw.PineBlog.Models;
using Opw.PineBlog.Posts;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Areas.Blog.Pages
{
    public class IndexModel : PageModelBase<IndexModel>
    {
        private readonly IMediator _mediator;

        public PostListModel Model { get; private set; }

        [ViewData]
        public string Title { get; private set; }

        public IndexModel(IMediator mediator, ILogger<IndexModel> logger) : base(logger)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken, [FromQuery]int page = 1)
        {
            var result = await _mediator.Send(new GetPagedPostListQuery { Page = page }, cancellationToken);

            Model = result.Value;
            Title = Model.Blog.Title;

            return Page();
        }
    }
}