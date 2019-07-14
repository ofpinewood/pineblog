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
        /// Handler for the GetPagedPostListQuery.
        /// </summary>
        public class Handler : IRequestHandler<GetPagedPostListQuery, Result<PostListModel>>
        {
            private readonly IOptions<BlogOptions> _blogOptions;
            private readonly IBlogEntityDbContext _context;

            /// <summary>
            /// Implementation of GetPagedPostListQuery.Handler.
            /// </summary>
            /// <param name="context">The blog entity context.</param>
            /// <param name="blogOptions">The blog options.</param>
            public Handler(IBlogEntityDbContext context, IOptions<BlogOptions> blogOptions)
            {
                _blogOptions = blogOptions;
                _context = context;
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
                var posts = request.IncludeUnpublished
                    ? await GetPagedListAsync(null, pager, cancellationToken)
                    : await GetPagedListAsync(p => p.Published != null, pager, cancellationToken);

                var model = new PostListModel
                {
                    Blog = new BlogModel(_blogOptions.Value),
                    PostListType = PostListType.Blog,
                    Posts = posts,
                    Pager = pager
                };

                return Result<PostListModel>.Success(model);
            }

            private async Task<IEnumerable<Post>> GetPagedListAsync(Expression<Func<Post, bool>> predicate, Pager pager, CancellationToken cancellationToken)
            {
                var skip = (pager.CurrentPage - 1) * pager.ItemsPerPage;

                var count = (predicate != null)
                    ? await _context.Posts.Where(predicate).CountAsync(cancellationToken)
                    : await _context.Posts.CountAsync(cancellationToken);

                pager.Configure(count, _blogOptions.Value.PagingUrlPartFormat);

                var query = _context.Posts
                    .Include(p => p.Author)
                    .Include(p => p.Cover)
                    .OrderByDescending(p => p.Published)
                    .Skip(skip)
                    .Take(pager.ItemsPerPage);

                if (predicate != null)
                    query = query.Where(predicate);

                return await query.ToListAsync(cancellationToken);
            }
        }
    }
}
