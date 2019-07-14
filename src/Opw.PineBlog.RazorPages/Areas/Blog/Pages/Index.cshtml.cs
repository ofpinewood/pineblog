using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Opw.PineBlog.Entities;
using Opw.PineBlog.Models;
using Opw.PineBlog.Posts;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Areas.Blog.Pages
{
    public class IndexModel : PageModelBase<IndexModel>
    {
        private readonly IMediator _mediator;

        public BlogModel Blog { get; set; }
        public Pager Pager { get; private set; }
        public IEnumerable<Post> Posts { get; set; }

        [ViewData]
        public string Title { get; private set; }

        public IndexModel(IMediator mediator, ILogger<IndexModel> logger) : base(logger)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken, [FromQuery]int page = 1)
        {
            var result = await _mediator.Send(new GetPagedPostListQuery { Page = page }, cancellationToken);

            Blog = result.Value.Blog;
            Pager = result.Value.Pager;
            Posts = result.Value.Posts;
            
            Title = Blog.Title;

            return Page();
        }
    }
}