using MediatR;
using Microsoft.Extensions.Options;
using Opw.PineBlog.Entities;
using Opw.PineBlog.Files;
using Opw.PineBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Opw.PineBlog.Posts
{
    /// <summary>
    /// Query that searches posts.
    /// </summary>
    public class SearchPostsQuery : IRequest<Result<PostListModel>>
    {
        /// <summary>
        /// The requested page.
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// The number of items per page, if not set the BlogOptions.ItemsPerPage will be used.
        /// </summary>
        public int? ItemsPerPage { get; set; }

        /// <summary>
        /// Text to search for.
        /// </summary>
        public string SearchQuery { get; set; }

        /// <summary>
        /// Handler for the SearchPostsQuery.
        /// </summary>
        public class Handler : IRequestHandler<SearchPostsQuery, Result<PostListModel>>
        {
            private readonly IOptionsSnapshot<PineBlogOptions> _blogOptions;
            private readonly IBlogUnitOfWork _uow;
            private readonly PostUrlHelper _postUrlHelper;
            private readonly FileUrlHelper _fileUrlHelper;

            /// <summary>
            /// Implementation of SearchPostsQuery.Handler.
            /// </summary>
            /// <param name="uow">The blog unit of work.</param>
            /// <param name="blogOptions">The blog options.</param>
            /// <param name="postUrlHelper">Post URL helper.</param>
            /// <param name="fileUrlHelper">File URL helper.</param>
            public Handler(IBlogUnitOfWork uow, IOptionsSnapshot<PineBlogOptions> blogOptions, PostUrlHelper postUrlHelper, FileUrlHelper fileUrlHelper)
            {
                _blogOptions = blogOptions;
                _uow = uow;
                _postUrlHelper = postUrlHelper;
                _fileUrlHelper = fileUrlHelper;
            }

            /// <summary>
            /// Handle the SearchPostsQuery request.
            /// </summary>
            /// <param name="request">The SearchPostsQuery request.</param>
            /// <param name="cancellationToken">A cancellation token.</param>
            public async Task<Result<PostListModel>> Handle(SearchPostsQuery request, CancellationToken cancellationToken)
            {
                var itemsPerPage = (request.ItemsPerPage.HasValue) ? request.ItemsPerPage : _blogOptions.Value.ItemsPerPage;
                var pager = new Pager(request.Page, itemsPerPage.Value);

                var pagingUrlPartFormat = _blogOptions.Value.PagingUrlPartFormat;

                var predicates = new List<Expression<Func<Post, bool>>>();
                predicates.Add(p => p.Published != null);

                if (!string.IsNullOrWhiteSpace(request.SearchQuery))
                {
                    // TODO: add more weight to the title and categories
                    predicates.Add(p =>
                        p.Title.Contains(request.SearchQuery) ||
                        p.Categories.Contains(request.SearchQuery) ||
                        p.Description.Contains(request.SearchQuery) ||
                        p.Content.Contains(request.SearchQuery));

                    pagingUrlPartFormat += "&" + string.Format(_blogOptions.Value.SearchQueryUrlPartFormat, HttpUtility.UrlEncode(request.SearchQuery));
                }

                var posts = await GetPagedListAsync(predicates, pager, pagingUrlPartFormat, cancellationToken);

                posts = posts.Select(p => _postUrlHelper.ReplaceUrlFormatWithBaseUrl(p));

                var model = new PostListModel
                {
                    Blog = new BlogModel(_blogOptions.Value),
                    PostListType = PostListType.Blog,
                    Posts = posts,
                    Pager = pager
                };

                model.Blog.CoverUrl = _fileUrlHelper.ReplaceUrlFormatWithBaseUrl(model.Blog.CoverUrl);

                if (!string.IsNullOrWhiteSpace(request.SearchQuery))
                {
                    model.PostListType = PostListType.Search;
                    model.SearchQuery = request.SearchQuery;
                }

                return Result<PostListModel>.Success(model);
            }

            private async Task<IEnumerable<Post>> GetPagedListAsync(IEnumerable<Expression<Func<Post, bool>>> predicates, Pager pager, string pagingUrlPartFormat, CancellationToken cancellationToken)
            {
                var skip = (pager.CurrentPage - 1) * pager.ItemsPerPage;
                var count = await _uow.Posts.CountAsync(predicates, cancellationToken);

                pager.Configure(count, pagingUrlPartFormat);

                return await _uow.Posts.GetAsync(predicates, skip, pager.ItemsPerPage, cancellationToken);
            }
        }
    }
}
