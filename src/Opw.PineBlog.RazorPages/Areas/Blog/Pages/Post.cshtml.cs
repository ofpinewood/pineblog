using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Opw.PineBlog.FeatureManagement;
using Opw.PineBlog.Models;
using Opw.PineBlog.Posts;

namespace Opw.PineBlog.RazorPages.Areas.Blog.Pages
{
    public class PostModel : PageModelBase<PostModel>
    {
        private readonly IMediator _mediator;

        public PineBlog.Models.PostModel Post { get; set; }

        public Models.MetadataModel Metadata { get; set; }

        public Models.PageCoverModel PageCover { get; set; }

        [ViewData]
        public string BlogTitle { get; set; }

        [ViewData]
        public string Title { get; private set; }

        public PostModel(IMediator mediator, IFeatureManager featureManager, ILogger<PostModel> logger)
            : base(featureManager, logger)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> OnGetAsync(string slug, CancellationToken cancellationToken)
        {
            if (slug.EndsWith('/'))
            {
                // remove the '/' at the end of the slug to make it valid
                slug = slug.TrimEnd('/');
            }

            var result = await _mediator.Send(new GetPostQuery { Slug = slug }, cancellationToken);

            if (!result.IsSuccess) return Error(result);

            Post = result.Value;
            Title = result.Value.Post.Title;

            BlogTitle = Post.Blog.Title;

            Metadata = new Models.MetadataModel
            {
                Author = Post.Post.Author.DisplayName,
                Description = Post.Post.Description,
                Published = Post.Post.Published,
                Title = Post.Post.Title,
                Type = "article",
                Url = Request.GetEncodedUrl()
            };

            Metadata.Image = Post.Post.CoverUrl;
            if (Post.Post.CoverUrl != null && !Post.Post.CoverUrl.StartsWith("http", System.StringComparison.OrdinalIgnoreCase))
                Metadata.Image = $"{Request.Scheme}://{Request.Host}{Post.Post.CoverUrl}";
            if (!string.IsNullOrWhiteSpace(Post.Post.Categories))
                Metadata.Keywords = Post.Post.Categories?.Split(',');

            PageCover = new Models.PageCoverModel
            {
                Title = Post.Post.Title,
                CoverUrl = Post.Post.CoverUrl,
                CoverCaption = Post.Post.CoverCaption,
                CoverLink = Post.Post.CoverLink
            };

            return Page();
        }
    }
}
