using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Opw.PineBlog.Posts;

namespace Opw.PineBlog.RazorPages.Areas.Admin.Pages
{
    public class UpdatePostModel : PageModelBase<UpdatePostModel>
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public UpdatePostCommand Post { get; set; }

        public UpdatePostModel(IMediator mediator, ILogger<UpdatePostModel> logger)
            : base(logger)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> OnGetAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetPostByIdQuery { Id = id }, cancellationToken);
            if (!result.IsSuccess)
                throw result.Exception;

            Post = new UpdatePostCommand
            {
                Id = result.Value.Id,
                Title = result.Value.Title,
                Description = result.Value.Description,
                Content = result.Value.Content,
                Categories = result.Value.Categories,
                Published = result.Value.Published,
                CoverUrl = result.Value.CoverUrl,
                CoverCaption = result.Value.CoverCaption,
                CoverLink = result.Value.CoverLink
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _mediator.Send(Post, cancellationToken);
            if (!result.IsSuccess)
            {
                Logger.LogError(result.Exception, "Could not update post.");
                ModelState.AddModelError("", result.Exception.Message);
            }

            return Page();
        }

        public async Task<IActionResult> OnGetPublishAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new PublishPostCommand { Id = id }, cancellationToken);
            if (!result.IsSuccess)
                throw result.Exception;

            return RedirectToPage("UpdatePost", new { id });
        }

        public async Task<IActionResult> OnGetUnpublishAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new UnpublishPostCommand { Id = id }, cancellationToken);
            if (!result.IsSuccess)
                throw result.Exception;

            return RedirectToPage("UpdatePost", new { id });
        }

        public async Task<IActionResult> OnGetDeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeletePostCommand { Id = id }, cancellationToken);
            if (!result.IsSuccess)
                throw result.Exception;

            return RedirectToPage("Posts");
        }
    }
}
