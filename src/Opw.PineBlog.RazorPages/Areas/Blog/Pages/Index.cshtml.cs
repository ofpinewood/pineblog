using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Opw.PineBlog.Models;
using Opw.PineBlog.Posts;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.RazorPages.Areas.Blog.Pages
{
    public class IndexModel : PageModelBase<IndexModel>
    {
        private readonly IMediator _mediator;

        public PostListModel PostList { get; set; }

        public Models.MetadataModel Metadata { get; set; }

        public Models.PageCoverModel PageCover { get; set; }

        [ViewData]
        public string Title { get; private set; }

        public IndexModel(IMediator mediator, ILogger<IndexModel> logger)
            : base(logger)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken, [FromQuery]int page = 1, [FromQuery]string category = null)
        {
            var result = await _mediator.Send(new GetPagedPostListQuery { Page = page, Category = category }, cancellationToken);

            PostList = result.Value;
            Title = result.Value.Blog.Title;

            Metadata = new Models.MetadataModel
            {
                Description = PostList.Blog.Description,
                Title = PostList.Blog.Title,
                Type = "website",
                Url = Request.GetEncodedUrl()
            };

            Metadata.Image = PostList.Blog.CoverUrl;
            if (PostList.Blog.CoverUrl != null && !PostList.Blog.CoverUrl.StartsWith("http", System.StringComparison.OrdinalIgnoreCase))
                Metadata.Image = $"{Request.Scheme}://{Request.Host}{PostList.Blog.CoverUrl}";

            PageCover = new Models.PageCoverModel
            {
                Title = PostList.Blog.Title,
                CoverUrl = PostList.Blog.CoverUrl,
                CoverCaption = PostList.Blog.CoverCaption,
                CoverLink = PostList.Blog.CoverLink
            };

            return Page();
        }
    }
}