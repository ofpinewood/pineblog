using System;
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
    public class PostModel : PageModelBase<PostModel>
    {
        private readonly IMediator _mediator;

        public BlogModel Blog { get; set; }
        public Post Post { get; set; }

        public PostModel(IMediator mediator, ILogger<PostModel> logger) : base(logger)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> OnGetAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetPostByIdQuery { Id = id }, cancellationToken);

            if (!result.IsSuccess) return Error(result);

            Blog = result.Value.Blog;
            Post = result.Value.Post;

            return Page();
        }
    }
}
