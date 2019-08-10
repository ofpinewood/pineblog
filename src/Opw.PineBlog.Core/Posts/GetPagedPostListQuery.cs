using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Opw.PineBlog.Entities;
using Opw.PineBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Posts
{
    /// <summary>
    /// Query that gets a PostListModel.
    /// </summary>
    public class GetPagedPostListQuery : IRequest<Result<PostListModel>>
    {
        /// <summary>
        /// The requested page.
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Include unpublished posts or not.
        /// </summary>
        public bool IncludeUnpublished { get; set; }

        /// <summary>
        /// The number of items per page, if not set the BlogOptions.ItemsPerPage will be used.
        /// </summary>
        public int? ItemsPerPage { get; set; }

        /// <summary>
        /// Category to filter on.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Handler for the GetPagedPostListQuery.
        /// </summary>
        public class Handler : IRequestHandler<GetPagedPostListQuery, Result<PostListModel>>
        {
            private readonly IOptions<PineBlogOptions> _blogOptions;
            private readonly IBlogEntityDbContext _context;
            private readonly PostUrlHelper _postUrlHelper;

            /// <summary>
            /// Implementation of GetPagedPostListQuery.Handler.
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
            /// Handle the GetPagedPostListQuery request.
            /// </summary>
            /// <param name="request">The GetPagedPostListQuery request.</param>
            /// <param name="cancellationToken">A cancellation token.</param>
            public async Task<Result<PostListModel>> Handle(GetPagedPostListQuery request, CancellationToken cancellationToken)
            {
                var itemsPerPage = (request.ItemsPerPage.HasValue) ? request.ItemsPerPage : _blogOptions.Value.ItemsPerPage;
                var pager = new Pager(request.Page, itemsPerPage.Value);

                var pagingUrlPartFormat = _blogOptions.Value.PagingUrlPartFormat;

                var predicates = new List<Expression<Func<Post, bool>>>();
                if (!request.IncludeUnpublished)
                    predicates.Add(p => p.Published != null);
                if (!string.IsNullOrWhiteSpace(request.Category))
                {
                    predicates.Add(p => p.Categories.Contains(request.Category));
                    pagingUrlPartFormat += "&" + string.Format(_blogOptions.Value.CategoryUrlPartFormat, request.Category);
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

                if (!string.IsNullOrWhiteSpace(request.Category))
                {
                    model.PostListType = PostListType.Category;
                    model.Category = request.Category;
                }

                return Result<PostListModel>.Success(model);
            }

            private async Task<IEnumerable<Post>> GetPagedListAsync(IEnumerable<Expression<Func<Post, bool>>> predicates, Pager pager, string pagingUrlPartFormat, CancellationToken cancellationToken)
            {
                var skip = (pager.CurrentPage - 1) * pager.ItemsPerPage;

                var countQuery = _context.Posts.Where(_ => true);
                foreach(var predicate in predicates)
                    countQuery = countQuery.Where(predicate);

                var count = await countQuery.CountAsync(cancellationToken);

                pager.Configure(count, pagingUrlPartFormat);

                var query = _context.Posts.Where(_ => true);

                foreach (var predicate in predicates)
                    query = query.Where(predicate);

                query = query.Include(p => p.Author)
                    .OrderByDescending(p => p.Published)
                    .Skip(skip)
                    .Take(pager.ItemsPerPage);

                return await query.ToListAsync(cancellationToken);
            }
        }
    }
}
