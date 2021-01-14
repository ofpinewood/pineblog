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
        private readonly IOptionsSnapshot<PineBlogOptions> _options;

        public PostListModel PostList { get; set; }

        public Models.MetadataModel Metadata { get; set; }

        public Models.PageCoverModel PageCover { get; set; }

        [ViewData]
        public bool ShowSearch { get; set; }

        [ViewData]
        public string SearchQuery { get; set; }

        [ViewData]
        public string Title { get; private set; }

        [ViewData]
        public string BlogTitle { get; private set; }

        public IndexModel(IMediator mediator, IOptionsSnapshot<PineBlogOptions> options, ILogger<IndexModel> logger)
            : base(logger)
        {
            _mediator = mediator;
            _options = options;
        }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken, [FromQuery] int page = 1, [FromQuery] string category = null, [FromQuery] string q = null)
        {
            Result<PostListModel> result;
            if (!string.IsNullOrWhiteSpace(q))
                result = await _mediator.Send(new SearchPostsQuery { Page = page, SearchQuery = q }, cancellationToken);
            else
                result = await _mediator.Send(new GetPagedPostListQuery { Page = page, Category = category }, cancellationToken);

            return GetPage(result);
        }

        private IActionResult GetPage(Result<PostListModel> result)
        {
            PostList = result.Value;
            Title = result.Value.Blog.Title;
            ShowSearch = _options.Value.EnableSearch;

            if (PostList.PostListType == PostListType.Search)
            {
                BlogTitle = result.Value.Blog.Title;
                SearchQuery = PostList.SearchQuery;
            }

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
                PostListType = PostList.PostListType,
                Category = PostList.Category,
                Title = PostList.Blog.Title,
                CoverUrl = PostList.Blog.CoverUrl,
                CoverCaption = PostList.Blog.CoverCaption,
                CoverLink = PostList.Blog.CoverLink,
            };

            return Page();
        }
    }
}