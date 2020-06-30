using Opw.PineBlog.Entities;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using Opw.PineBlog.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Opw.PineBlog.EntityFrameworkCore.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly BlogEntityDbContext _dbContext;

        public PostRepository(BlogEntityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Post> SingleOrDefaultAsync(Expression<Func<Post, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _dbContext.Posts
                .Include(p => p.Author)
                .SingleOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<Post> GetNextAsync(DateTime published, CancellationToken cancellationToken)
        {
            return await _dbContext.Posts
                .Where(p => p.Published > published)
                .OrderBy(p => p.Published)
                .Take(1)
                .SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<Post> GetPreviousAsync(DateTime published, CancellationToken cancellationToken)
        {
            return await _dbContext.Posts
                .Where(p => p.Published < published)
                .OrderByDescending(p => p.Published)
                .Take(1)
                .SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<Post>> GetPublishedAsync(int take, CancellationToken cancellationToken)
        {
            var query = _dbContext.Posts
                .Include(p => p.Author)
                .Where(p => p.Published != null)
                .OrderByDescending(p => p.Published)
                .Take(take);

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(IEnumerable<Expression<Func<Post, bool>>> predicates, CancellationToken cancellationToken)
        {
            var query = _dbContext.Posts.Where(_ => true);
            foreach (var predicate in predicates)
                query = query.Where(predicate);

            return await query.CountAsync(cancellationToken);
        }

        public async Task<IEnumerable<Post>> GetAsync(IEnumerable<Expression<Func<Post, bool>>> predicates, int skip, int take, CancellationToken cancellationToken)
        {
            var query = _dbContext.Posts.Where(_ => true);

            foreach (var predicate in predicates)
                query = query.Where(predicate);

            query = query.Include(p => p.Author)
                .OrderByDescending(p => p.Published)
                .Skip(skip)
                .Take(take);

            return await query.ToListAsync(cancellationToken);
        }

        public Post Add([NotNull] Post post)
        {
            var entry = _dbContext.Posts.Add(post);
            return entry.Entity;
        }

        public Post Update([NotNull] Post post)
        {
            var entry = _dbContext.Posts.Update(post);
            return entry.Entity;
        }

        public Post Remove([NotNull] Post post)
        {
            var entry = _dbContext.Posts.Remove(post);
            return entry.Entity;
        }
    }
}
