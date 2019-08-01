using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Opw.PineBlog.Entities;
using Opw.PineBlog.Posts;

namespace Opw.PineBlog.Areas.Admin.Pages
{
    public class AddPostModel : PageModelBase<AddPostModel>
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public AddPostCommand Post { get; set; }

        public Cover Cover { get; set; }

        public AddPostModel(IMediator mediator, ILogger<AddPostModel> logger) : base(logger)
        {
            _mediator = mediator;
        }

        public IActionResult OnGet()
        {
            Post = new AddPostCommand();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            ModelState.Remove($"{nameof(Post)}.{nameof(Post.UserName)}");
            if (!ModelState.IsValid)
                return Page();

            Post.UserName = User.Identity.Name;

            var result = await _mediator.Send(Post, cancellationToken);
            if (!result.IsSuccess)
            {
                Post.UserName = null;
                ModelState.AddModelError("", result.Exception.Message);
                return Page();
            }

            return RedirectToPage("Posts");
        }
    }
}