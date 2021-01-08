using Opw.PineBlog.Entities;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using Opw.PineBlog.Repositories;
using MongoDB.Driver;
using System.Linq;

namespace Opw.PineBlog.MongoDb.Repositories
{
    public class PostRepository : RepositoryBase<Post>, IPostRepository
    {
        private readonly BlogUnitOfWork _uow;

        private IMongoCollection<Author> _authorCollection;
        protected IMongoCollection<Author> AuthorCollection
        {
            get
            {
                if (_authorCollection == null)
                    _authorCollection = _uow.Database.GetCollection<Author>(CollectionHelper.GetName<Author>());
                return _authorCollection;
            }
        }

        public PostRepository(BlogUnitOfWork uow) : base(uow)
        {
            _uow = uow;
        }

        public async Task<Post> SingleOrDefaultAsync(Expression<Func<Post, bool>> predicate, CancellationToken cancellationToken)
        {
            var post = await Collection
                .Find(predicate)
                .SingleOrDefaultAsync(cancellationToken);

            if (post != null)
            {
                var author = await AuthorCollection
                    .Find(a => a.Id == post.AuthorId)
                    .SingleOrDefaultAsync(cancellationToken);
                post.Author = author;
            }

            return post;
        }

        public async Task<Post> GetNextAsync(DateTime published, CancellationToken cancellationToken)
        {
            return await Collection
                .Find(p => p.Published > published)
                .SortBy(p => p.Published)
                .Limit(1)
                .SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<Post> GetPreviousAsync(DateTime published, CancellationToken cancellationToken)
        {
            return await Collection
                .Find(p => p.Published < published)
                .SortByDescending(p => p.Published)
                .Limit(1)
                .SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<int> CountAsync(IEnumerable<Expression<Func<Post, bool>>> predicates, CancellationToken cancellationToken)
        {
            var filter = BuildFilter(predicates);

            var count = await Collection
                .Find(filter)
                .CountDocumentsAsync(cancellationToken);
            return (int)count;
        }

        public async Task<IEnumerable<Post>> GetAsync(IEnumerable<Expression<Func<Post, bool>>> predicates, int skip, int take, CancellationToken cancellationToken)
        {
            var filter = BuildFilter(predicates);

            var posts = await Collection
                .Find(filter)
                .SortByDescending(p => p.Published)
                .Skip(skip)
                .Limit(take)
                .ToListAsync(cancellationToken);

            if (posts.Any())
            {
                var authorIds = posts.Select(p => p.AuthorId).Distinct();
                var authors = await AuthorCollection
                    .Find(a => authorIds.Contains(a.Id))
                    .ToListAsync(cancellationToken);

                foreach (var post in posts)
                {
                    post.Author = authors.SingleOrDefault(a => a.Id == post.AuthorId);
                }
            }

            return posts;
        }

        public new Post Add([NotNull] Post post)
        {
            post.Author = null;
            return base.Add(post);
        }

        public Post Update([NotNull] Post post)
        {
            post.Author = null;
            return Update(p => p.Id == post.Id, post);
        }

        public Post Remove([NotNull] Post post)
        {
            Remove(p => p.Id == post.Id);
            return post;
        }

        private FilterDefinition<Post> BuildFilter(IEnumerable<Expression<Func<Post, bool>>> predicates)
        {
            var filter = Builders<Post>.Filter.Where(_ => true);

            if (predicates.Any())
            {
                var filterList = new List<FilterDefinition<Post>>();
                foreach (var predicate in predicates)
                {
                    filterList.Add(Builders<Post>.Filter.Where(predicate));
                }

                filter = Builders<Post>.Filter.And(filterList);
            }

            return filter;
        }
    }
}
