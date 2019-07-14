using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Opw.PineBlog.Entities;
using Opw.PineBlog.Models;
using Opw.PineBlog.Posts;

namespace Opw.PineBlog.Areas.Admin.Pages
{
    public class PostsModel : PageModelBase<PostsModel>
    {
        private readonly IMediator _mediator;

        public BlogModel Blog { get; set; }
        public Pager Pager { get; private set; }
        public IEnumerable<Post> Posts { get; set; }

        public PostsModel(IMediator mediator, ILogger<PostsModel> logger) : base(logger)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken, [FromQuery]int page = 1)
        {
            var result = await _mediator.Send(new GetPagedPostListQuery { Page = page, IncludeUnpublished = true, ItemsPerPage = 25 }, cancellationToken);

            Blog = result.Value.Blog;
            Pager = result.Value.Pager;
            Posts = result.Value.Posts;

            return Page();
        }
    }
}
