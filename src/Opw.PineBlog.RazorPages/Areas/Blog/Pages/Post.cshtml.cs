using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Opw.PineBlog.Entities;
using Opw.PineBlog.Models;
using Opw.PineBlog.Posts;

namespace Opw.PineBlog.Areas.Blog.Pages
{
    public class PostModel : PageModelBase<PostModel>
    {
        private readonly IMediator _mediator;

        public BlogModel Blog { get; set; }
        public Post Post { get; set; }
        public Post Previous { get; set; }
        public Post Next { get; set; }

        [ViewData]
        public string Title { get; private set; }

        public PostModel(IMediator mediator, ILogger<PostModel> logger) : base(logger)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> OnGetAsync(string slug, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetPostQuery { Slug = slug }, cancellationToken);

            if (!result.IsSuccess) return Error(result);

            Blog = result.Value.Blog;
            Post = result.Value.Post;
            Previous = result.Value.Previous;
            Next = result.Value.Next;

            Title = Blog.Title;

            return Page();
        }
    }
}
