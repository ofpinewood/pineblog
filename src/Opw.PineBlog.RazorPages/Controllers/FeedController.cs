using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Opw.PineBlog.Feeds;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.RazorPages.Controllers
{
    /// <summary>
    /// API controller for returning RSS and ATOM feeds.
    /// </summary>
    [ApiController]
    [Route(PineBlogConstants.BlogAreaPath + "/[controller]")]
    public class FeedController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<FeedController> _logger;

        /// <summary>
        /// Implementation of FeedController.
        /// </summary>
        /// <param name="mediator">Mediator.</param>
        /// <param name="logger">Logger.</param>
        public FeedController(
            IMediator mediator,
            ILogger<FeedController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Get the RSS feed.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        [HttpGet("rss")]
        [Produces("application/rss+xml")]
        public async Task<IActionResult> Rss(CancellationToken cancellationToken)
        {
            try
            {
                return await GetFeed(FeedType.Rss, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RSS feed error.");
                return StatusCode(StatusCodes.Status500InternalServerError, "RSS feed error.");
            }
        }

        /// <summary>
        /// Get the RSS feed.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        [HttpGet("atom")]
        [Produces("application/atom+xml")]
        public async Task<IActionResult> Atom(CancellationToken cancellationToken)
        {
            try
            {
                return await GetFeed(FeedType.Atom, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Atom feed error.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Atom feed error.");
            }
        }

        private async Task<IActionResult> GetFeed(FeedType feedType, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetSyndicationFeedQuery
            {
                FeedType = feedType,
                BaseUri = new Uri(Request.Scheme + "://" + Request.Host),
                PostBasePath = PineBlogConstants.BlogAreaPath
            }, cancellationToken);

            if (!result.IsSuccess)
                throw result.Exception;

            return Content(result.Value.Feed, result.Value.ContentType);
        }
    }
}
