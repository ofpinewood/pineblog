using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Opw.PineBlog.Covers;
using Opw.PineBlog.Entities;
using Opw.PineBlog.Models;

namespace Opw.PineBlog.Areas.Admin.Pages
{
    public class CoversModel : PageModelBase<CoversModel>
    {
        private readonly IMediator _mediator;

        public Pager Pager { get; private set; }
        public IEnumerable<Cover> Covers { get; set; }

        public CoversModel(IMediator mediator, ILogger<CoversModel> logger) : base(logger)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken, [FromQuery]int page = 1)
        {
			var result = await _mediator.Send(new GetPagedCoverListQuery { Page = page, ItemsPerPage = 25 }, cancellationToken);

			Pager = result.Value.Pager;
			Covers = result.Value.Covers;

			return Page();
        }
    }
}
