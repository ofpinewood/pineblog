using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Opw.PineBlog.Entities;
using Opw.PineBlog.FeatureManagement;
using Opw.PineBlog.Models;
using Opw.PineBlog.Posts;

namespace Opw.PineBlog.RazorPages.Areas.Admin.Pages
{
    public class PostsModel : PageModelBase<PostsModel>
    {
        private readonly IMediator _mediator;

        public Pager Pager { get; private set; }
        public IEnumerable<Post> Posts { get; set; }

        public PostsModel(IMediator mediator, IFeatureManager featureManager, ILogger<PostsModel> logger)
            : base(featureManager, logger)
        {
            _mediator = mediator;

            FeatureState = FeatureManager.IsEnabled(FeatureFlag.AdminPosts);
        }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken, [FromQuery]int page = 1)
        {
            var result = await _mediator.Send(new GetPagedPostListQuery { Page = page, IncludeUnpublished = true, ItemsPerPage = 10 }, cancellationToken);

            Pager = result.Value.Pager;
            Posts = result.Value.Posts;

            return Page();
        }
    }
}
