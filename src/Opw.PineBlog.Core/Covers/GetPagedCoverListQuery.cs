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

namespace Opw.PineBlog.Covers
{
    /// <summary>
    /// Query that gets a CoverListModel.
    /// </summary>
    public class GetPagedCoverListQuery : IRequest<Result<CoverListModel>>
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
        /// Handler for the GetPagedCoverListQuery.
        /// </summary>
        public class Handler : IRequestHandler<GetPagedCoverListQuery, Result<CoverListModel>>
        {
            private readonly IOptions<PineBlogOptions> _blogOptions;
            private readonly IBlogEntityDbContext _context;

            /// <summary>
            /// Implementation of GetPagedCoverListQuery.Handler.
            /// </summary>
            /// <param name="context">The blog entity context.</param>
            /// <param name="blogOptions">The blog options.</param>
            public Handler(IBlogEntityDbContext context, IOptions<PineBlogOptions> blogOptions)
            {
                _blogOptions = blogOptions;
                _context = context;
            }

            /// <summary>
            /// Handle the GetPagedCoverListQuery request.
            /// </summary>
            /// <param name="request">The GetPagedCoverListQuery request.</param>
            /// <param name="cancellationToken">A cancellation token.</param>
            public async Task<Result<CoverListModel>> Handle(GetPagedCoverListQuery request, CancellationToken cancellationToken)
            {
                var itemsPerPage = (request.ItemsPerPage.HasValue) ? request.ItemsPerPage : _blogOptions.Value.ItemsPerPage;
                var pager = new Pager(request.Page, itemsPerPage.Value);
                var covers = await GetPagedListAsync(null, pager, cancellationToken);

                var model = new CoverListModel
                {
                    Covers = covers,
                    Pager = pager
                };

                return Result<CoverListModel>.Success(model);
            }

            private async Task<IEnumerable<Cover>> GetPagedListAsync(Expression<Func<Cover, bool>> predicate, Pager pager, CancellationToken cancellationToken)
            {
                var skip = (pager.CurrentPage - 1) * pager.ItemsPerPage;

                var count = (predicate != null)
                    ? await _context.Covers.Where(predicate).CountAsync(cancellationToken)
                    : await _context.Covers.CountAsync(cancellationToken);

                pager.Configure(count, _blogOptions.Value.PagingUrlPartFormat);

                var query = _context.Covers
                    .Include(c => c.Posts)
                    .OrderByDescending(p => p.Created)
                    .Skip(skip)
                    .Take(pager.ItemsPerPage);

                if (predicate != null)
                    query = query.Where(predicate);

                return await query.ToListAsync(cancellationToken);
            }
        }
    }
}
