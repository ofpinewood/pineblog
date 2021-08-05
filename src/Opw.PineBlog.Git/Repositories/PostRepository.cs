using Opw.PineBlog.Entities;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using Opw.PineBlog.Repositories;
using System.Linq;
using Opw.PineBlog.Git.LibGit2;

namespace Opw.PineBlog.Git.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly GitContext _gitContext;

        public PostRepository(GitContext gitContext)
        {
            _gitContext = gitContext;
        }

        public async Task<Post> SingleOrDefaultAsync(Expression<Func<Post, bool>> predicate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            //return await _dbContext.Posts
                //.Include(p => p.Author)
                //.SingleOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<Post> GetNextAsync(DateTime published, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            //return await _dbContext.Posts
                //.Where(p => p.Published > published)
                //.OrderBy(p => p.Published)
                //.Take(1)
                //.SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<Post> GetPreviousAsync(DateTime published, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            //return await _dbContext.Posts
                //.Where(p => p.Published < published)
                //.OrderByDescending(p => p.Published)
                //.Take(1)
                //.SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<int> CountAsync(IEnumerable<Expression<Func<Post, bool>>> predicates, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            //var query = _dbContext.Posts.Where(_ => true);
            //foreach (var predicate in predicates)
            //    query = query.Where(predicate);

            //return await query.CountAsync(cancellationToken);
        }

        public async Task<IEnumerable<Post>> GetAsync(IEnumerable<Expression<Func<Post, bool>>> predicates, int skip, int take, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            //var query = _dbContext.Posts.Where(_ => true);

            //foreach (var predicate in predicates)
            //    query = query.Where(predicate);

            //query = query.Include(p => p.Author)
            //    .OrderByDescending(p => p.Published)
            //    .Skip(skip)
            //    .Take(take);

            //return await query.ToListAsync(cancellationToken);
        }

        public Post Add([NotNull] Post post)
        {
            throw new NotImplementedException();
        }

        public Post Update([NotNull] Post post)
        {
            throw new NotImplementedException();
        }

        public Post Remove([NotNull] Post post)
        {
            throw new NotImplementedException();
        }
    }
}
