using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Opw.PineBlog.Entities;
using Opw.PineBlog.Models;
using Opw.PineBlog.Posts;

namespace Opw.PineBlog.Areas.Admin.Pages
{
    public class PostModel : PageModelBase<PostModel>
    {
        private readonly IMediator _mediator;
        private readonly IOptions<BlogOptions> _blogOptions;

        public BlogModel Blog { get; set; }
        public Post Post { get; set; }

        public Guid? Id { get; set; }

        public PostModel(IMediator mediator, IOptions<BlogOptions> blogOptions, ILogger<PostModel> logger) : base(logger)
        {
            _mediator = mediator;
            _blogOptions = blogOptions;
        }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken, Guid? id)
        {
            Id = id;

            SinglePostModel model;
            if (Id.HasValue)
            {
                var result = await _mediator.Send(new GetPostByIdQuery { Id = Id.Value }, cancellationToken);
                if (!result.IsSuccess) return Error(result);
                model = result.Value;
            } else
            {
                model = new SinglePostModel
                {
                    Blog = new BlogModel(_blogOptions.Value),
                    Post = new Post()
                };
            }
            
            Blog = model.Blog;
            Post = model.Post;

            return Page();
        }
    }
}
