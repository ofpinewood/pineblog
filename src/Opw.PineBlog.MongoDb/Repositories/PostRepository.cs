using Opw.PineBlog.Entities;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using Opw.PineBlog.Repositories;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Opw.PineBlog.MongoDb.Repositories
{
    // TODO: make sure the post document is complete; that it has all the required child objects, like author
    public class PostRepository : IPostRepository
    {
        private readonly BlogUnitOfWork _uow;
        private readonly IMongoCollection<Post> _collection;

        public PostRepository(BlogUnitOfWork uow)
        {
            _uow = uow;
            _collection = _uow.Database.GetCollection<Post>($"{nameof(Post)}s");
        }

        public async Task<Post> SingleOrDefaultAsync(Expression<Func<Post, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _collection
                .Find(predicate)
                .SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<Post> GetNextAsync(DateTime published, CancellationToken cancellationToken)
        {
            return await _collection
                .Find(p => p.Published > published)
                .SortBy(p => p.Published)
                .Limit(1)
                .SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<Post> GetPreviousAsync(DateTime published, CancellationToken cancellationToken)
        {
            return await _collection
                .Find(p => p.Published < published)
                .SortByDescending(p => p.Published)
                .Limit(1)
                .SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<Post>> GetPublishedAsync(int take, CancellationToken cancellationToken)
        {
            var query = _collection
                .Find(p => p.Published != null)
                .SortByDescending(p => p.Published)
                .Limit(take);

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(IEnumerable<Expression<Func<Post, bool>>> predicates, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            // TODO: implement count
            //var query = _dbContext.Posts.Where(_ => true);
            //foreach (var predicate in predicates)
            //    query = query.Where(predicate);

            //return await query.CountAsync(cancellationToken);
        }

        public async Task<IEnumerable<Post>> GetAsync(IEnumerable<Expression<Func<Post, bool>>> predicates, int skip, int take, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            // TODO: implement get
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
            _collection.InsertOne(post);
            _uow.SaveChangeCount++;
            return post;
        }

        public Post Update([NotNull] Post post)
        {
            _collection.ReplaceOne(p => p.Id == post.Id, post);
            _uow.SaveChangeCount++;
            return post;
        }

        public Post Remove([NotNull] Post post)
        {
            _collection.DeleteOne(p => p.Id == post.Id);
            _uow.SaveChangeCount++;
            return post;
        }
    }
}
