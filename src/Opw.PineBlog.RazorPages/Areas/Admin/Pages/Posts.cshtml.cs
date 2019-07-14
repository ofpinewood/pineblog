using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Opw.PineBlog.Entities;

namespace Opw.PineBlog.Areas.Admin.Pages
{
    public class PostsModel : PageModelBase<PostsModel>
    {
        private readonly IMediator _mediator;

        public IEnumerable<Post> Model { get; private set; }

        public PostsModel(IMediator mediator, ILogger<PostsModel> logger) : base(logger)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken, [FromQuery]int page = 1)
        {
            //var result = await _mediator.Send(new GetPagedPostsQuery { Page = page }, cancellationToken);

            //Model = result.Value;

            return Page();
        }
    }
}
