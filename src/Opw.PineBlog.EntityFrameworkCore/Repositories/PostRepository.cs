//using Microsoft.EntityFrameworkCore;
//using Opw.PineBlog.Models;
//using Opw.PineBlog.Repositories;
//using Opw.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Opw.PineBlog.EntityFrameworkCore.Repositories
//{
//    public class PostRepository : Repository<Post>, IPostRepository
//    {
//        readonly BlogEntityDbContext _dbContext;

//        public PostRepository(BlogEntityDbContext dbContext) : base(dbContext)
//        {
//            _dbContext = dbContext;
//        }

//        public async Task<IEnumerable<Post>> GetListAsync(Expression<Func<Post, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            cancellationToken.ThrowIfCancellationRequested();

//            var posts = await _dbContext.Posts
//                .Include(p => p.Author)
//                .Where(predicate)
//                .OrderByDescending(p => p.Published)
//                .ToListAsync(cancellationToken);

//            return posts;
//        }

//        public async Task<IEnumerable<Post>> GetPagedListAsync(Expression<Func<Post, bool>> predicate, Pager pager, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            cancellationToken.ThrowIfCancellationRequested();

//            var skip = pager.CurrentPage * pager.ItemsPerPage - pager.ItemsPerPage;

//            var count = await _dbContext.Posts
//                .Include(p => p.Author)
//                .Where(predicate)
//                .OrderByDescending(p => p.Published)
//                .CountAsync(cancellationToken);
//            pager.Configure(count);

//            var posts = await _dbContext.Posts
//                .Include(p => p.Author)
//                .Where(predicate)
//                .OrderByDescending(p => p.Published)
//                .Skip(skip)
//                .Take(pager.ItemsPerPage)
//                .ToListAsync(cancellationToken);
            
//            return posts;
//        }

//        public Task<IEnumerable<Post>> SearchAsync(Pager pager, string term, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            cancellationToken.ThrowIfCancellationRequested();

//            throw new NotImplementedException();
//        }

//        public async Task<Post> GetPostAsync(string slug, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            cancellationToken.ThrowIfCancellationRequested();

//            var post = await _dbContext.Posts
//                .Include(p => p.Author)
//                .Where(p => p.Published != null)
//                .Where(p => p.Slug.Equals(slug))
//                .OrderByDescending(p => p.Published)
//                .SingleOrDefaultAsync(cancellationToken);

//            return post;
//        }
//    }
//}
