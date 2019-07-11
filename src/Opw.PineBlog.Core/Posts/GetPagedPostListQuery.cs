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
    public class GetPagedPostListQuery : IRequest<Result<PostListModel>>
    {
        public int Page { get; set; }

        public class Handler : IRequestHandler<GetPagedPostListQuery, Result<PostListModel>>
        {
            private readonly IOptions<BlogOptions> _blogOptions;
            private readonly IBlogEntityDbContext _context;

            public Handler(IBlogEntityDbContext context, IOptions<BlogOptions> blogOptions)
            {
                _blogOptions = blogOptions;
                _context = context;
            }

            public async Task<Result<PostListModel>> Handle(GetPagedPostListQuery request, CancellationToken cancellationToken)
            {
                var pager = new Pager(request.Page, _blogOptions.Value.ItemsPerPage);
                var posts = await GetPagedListAsync(p => p.Published != null, pager, cancellationToken);

                var model = new PostListModel
                {
                    Blog = new BlogModel(_blogOptions.Value),
                    PostListType = PostListType.Blog,
                    Posts = posts,
                    Pager = pager
                };

                return Result<PostListModel>.Success(model);
            }

            public async Task<IEnumerable<Post>> GetPagedListAsync(Expression<Func<Post, bool>> predicate, Pager pager, CancellationToken cancellationToken)
            {
                var skip = pager.CurrentPage * pager.ItemsPerPage;

                var count = await _context.Posts
                    .Where(predicate)
                    .CountAsync(cancellationToken);

                pager.Configure(count, _blogOptions.Value.PostUrlFormat);

                return await _context.Posts
                    .Include(p => p.Author)
                    .Include(p => p.Cover)
                    .Where(predicate)
                    .OrderByDescending(p => p.Published)
                    .Skip(skip)
                    .Take(pager.ItemsPerPage)
                    .ToListAsync(cancellationToken);
            }
        }
    }
}
