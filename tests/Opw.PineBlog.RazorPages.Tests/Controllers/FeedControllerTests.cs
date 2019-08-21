using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using Opw.PineBlog.Feeds;
using Opw.PineBlog.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.RazorPages.Controllers
{
    public class FeedControllerTests
    {
        private readonly FeedController _controller;

        public FeedControllerTests()
        {
            var loggerMock = new Mock<ILogger<FeedController>>();
            var mediaterMock = new Mock<IMediator>();
            mediaterMock.Setup(m => m.Send(It.IsAny<GetSyndicationFeedQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetSyndicationFeedQuery request, CancellationToken _) => Result<FeedModel<SyndicationFeedFormatter>>.Success(GetFeedModel(request.FeedType)));

            _controller = new FeedController(mediaterMock.Object, loggerMock.Object);
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "http";
            httpContext.Request.Host = new HostString("localhost:5001");
            var actionContext = new ActionContext(httpContext, new RouteData(), new ControllerActionDescriptor(), new ModelStateDictionary());
            _controller.ControllerContext = new ControllerContext(actionContext);
        }

        [Fact]
        public async Task Rss_Should_ReturnRssFeed()
        {
            var result = await _controller.Rss(default);

            var contentResult = result.Should().BeOfType<ContentResult>().Which;

            contentResult.ContentType.Should().Be("application/rss+xml");
            contentResult.Content.Should().StartWith("<rss xmlns:a10=\"http://www.w3.org/2005/Atom\" version=\"2.0\">");
        }

        [Fact]
        public async Task Atom_Should_ReturnAtomFeed()
        {
            var result = await _controller.Atom(default);

            var contentResult = result.Should().BeOfType<ContentResult>().Which;

            contentResult.ContentType.Should().Be("application/atom+xml");
            contentResult.Content.Should().StartWith("<feed xml:base=\"http://www.example.com/\" xmlns=\"http://www.w3.org/2005/Atom\">");
        }

        private FeedModel<SyndicationFeedFormatter> GetFeedModel(FeedType feedType)
        {
            var model = new FeedModel<SyndicationFeedFormatter>();
            var postUrl = new Uri("http://www.example.com/posts/post-title");

            var feed = new SyndicationFeed
            {
                Title = new TextSyndicationContent("Blog title"),
                Description = new TextSyndicationContent("Latest blog posts"),
                BaseUri = new Uri("http://www.example.com"),
                Items = new List<SyndicationItem>
                {
                    new SyndicationItem("Post title", "Post content", postUrl, postUrl.ToString(), DateTime.UtcNow)
                },
            };

            if (feedType == FeedType.Atom)
            {
                model.ContentType = "application/atom+xml";
                model.Feed = new Atom10FeedFormatter(feed);
            }
            else if (feedType == FeedType.Rss)
            {
                model.ContentType = "application/rss+xml";
                model.Feed = new Rss20FeedFormatter(feed);
            }

            return model;
        }
    }
}
