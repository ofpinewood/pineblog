using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Opw.PineBlog.Models;
using Opw.PineBlog.Posts;
using System.Threading.Tasks;

namespace Opw.PineBlog.Blog.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public PostListModel Model { get; private set; }

        [ViewData]
        public string Title { get; private set; }

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> OnGetAsync([FromQuery]int page = 0)
        {
            var result = await _mediator.Send(new GetPagedPostListQuery { Page = page });

            Model = result.Value;
            Title = Model.Blog.Title;

            return Page();
        }
    }
}