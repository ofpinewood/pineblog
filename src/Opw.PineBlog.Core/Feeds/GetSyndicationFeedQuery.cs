using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Opw.PineBlog.Models;
using Opw.PineBlog.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Feeds
{
    /// <summary>
    /// Query that gets a syndication feed.
    /// </summary>
    public class GetSyndicationFeedQuery : IRequest<Result<FeedModel<SyndicationFeedFormatter>>>
    {
        /// <summary>
        /// The type of feed to return.
        /// </summary>
        public FeedType FeedType { get; set; }

        /// <summary>
        /// Base URL.
        /// </summary>
        public Uri BaseUri { get; set; }

        /// <summary>
        /// Absolute path for posts.
        /// </summary>
        public string PostBasePath { get; set; }

        /// <summary>
        /// Handler for the GetFeedQuery.
        /// </summary>
        public class Handler : IRequestHandler<GetSyndicationFeedQuery, Result<FeedModel<SyndicationFeedFormatter>>>
        {
            private readonly IOptions<PineBlogOptions> _blogOptions;
            private readonly IBlogEntityDbContext _context;
            private readonly PostUrlHelper _postUrlHelper;

            /// <summary>
            /// Implementation of GetFeedQuery.Handler.
            /// </summary>
            /// <param name="context">The blog entity context.</param>
            /// <param name="blogOptions">The blog options.</param>
            /// <param name="postUrlHelper">Post URL helper.</param>
            public Handler(IBlogEntityDbContext context, IOptions<PineBlogOptions> blogOptions, PostUrlHelper postUrlHelper)
            {
                _blogOptions = blogOptions;
                _context = context;
                _postUrlHelper = postUrlHelper;
            }

            /// <summary>
            /// Handle the GetFeedQuery request.
            /// </summary>
            /// <param name="request">The GetFeedQuery request.</param>
            /// <param name="cancellationToken">A cancellation token.</param>
            public async Task<Result<FeedModel<SyndicationFeedFormatter>>> Handle(GetSyndicationFeedQuery request, CancellationToken cancellationToken)
            {
                var feed = await GetFeedAsync(request, cancellationToken);
                var model = new FeedModel<SyndicationFeedFormatter>();

                if (request.FeedType == FeedType.Atom)
                {
                    model.ContentType = "application/atom+xml";
                    model.Feed = new Atom10FeedFormatter(feed);
                }
                else if (request.FeedType == FeedType.Rss)
                {
                    model.ContentType = "application/rss+xml";
                    model.Feed = new Rss20FeedFormatter(feed);
                }

                return Result<FeedModel<SyndicationFeedFormatter>>.Success(model);
            }

            private async Task<SyndicationFeed> GetFeedAsync(GetSyndicationFeedQuery request, CancellationToken cancellationToken)
            {
                var blog = new BlogModel(_blogOptions.Value);

                var feed = new SyndicationFeed()
                {
                    Title = new TextSyndicationContent(blog.Title),
                    Description = new TextSyndicationContent("Latest blog posts"),
                    BaseUri = request.BaseUri,
                    Items = await GetItemsAsync(request, cancellationToken),
                };

                // TODO: handle relative URLs, add a helper method for making them absolute. Also for the meta data on the home page
                if (!string.IsNullOrWhiteSpace(blog.CoverUrl) && blog.CoverUrl.StartsWith("http"))
                    feed.ImageUrl = new Uri(blog.CoverUrl);

                feed.Links.Add(new SyndicationLink(feed.BaseUri));
                feed.Copyright = new TextSyndicationContent("Copyright " + DateTime.UtcNow.Year);
                feed.LastUpdatedTime = DateTime.UtcNow;

                if (feed.Items.Any())
                    feed.Contributors.Add(feed.Items.First().Authors.First());

                return feed;
            }

            private async Task<IEnumerable<SyndicationItem>> GetItemsAsync(GetSyndicationFeedQuery request, CancellationToken cancellationToken)
            {
                var query = _context.Posts
                    .Include(p => p.Author)
                    .Where(p => p.Published != null)
                    .OrderByDescending(p => p.Published)
                    .Take(25);

                var posts = await query.ToListAsync(cancellationToken);
                var items = new List<SyndicationItem>();

                foreach (var post in posts.Select(p => _postUrlHelper.ReplaceUrlFormatWithBaseUrl(p)))
                {
                    var absoluteUrl = new Uri(request.BaseUri, $"{request.PostBasePath}/{post.Slug}");
                    var content = SyndicationContent.CreateHtmlContent(Markdig.Markdown.ToHtml(post.Content));

                    var item = new SyndicationItem(post.Title, content, absoluteUrl, absoluteUrl.ToString(), post.Modified)
                    {
                        PublishDate = post.Published.Value
                    };

                    if (!string.IsNullOrWhiteSpace(post.Description))
                        item.Summary = new TextSyndicationContent(post.Description);

                    if (!string.IsNullOrWhiteSpace(post.Categories))
                    {
                        foreach (var category in post.Categories.Split(','))
                        {
                            item.Categories.Add(new SyndicationCategory(category));
                        }
                    }

                    item.Authors.Add(new SyndicationPerson("", post.Author.DisplayName, request.BaseUri.ToString()));
                    items.Add(item);
                }

                return items;
            }
        }
    }
}
