using MongoDB.Driver;
using Opw.PineBlog.Entities;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Opw.PineBlog.MongoDb.Repositories
{
    public abstract class RepositoryBase<T>
    {
        private readonly BlogUnitOfWork _uow;

        private IMongoCollection<T> _collection;
        protected IMongoCollection<T> Collection
        {
            get
            {
                if (_collection == null)
                    _collection = _uow.Database.GetCollection<T>(CollectionHelper.GetName<T>());
                return _collection;
            }
        }

        public RepositoryBase(BlogUnitOfWork uow)
        {
            _uow = uow;
        }

        protected T Add([NotNull] T entity)
        {
            UpdateForCreate(entity);
            UpdateForModified(entity);

            Collection.InsertOne(entity);
            _uow.SaveChangeCount++;
            return entity;
        }

        protected T Update([NotNull] Expression<Func<T, bool>> predicate, T entity)
        {
            UpdateForModified(entity);

            Collection.ReplaceOne(predicate, entity);
            _uow.SaveChangeCount++;
            return entity;
        }

        protected void Remove([NotNull] Expression<Func<T, bool>> predicate)
        {
            var result = Collection.DeleteOne(predicate);
            _uow.SaveChangeCount += (int)result.DeletedCount;
        }

        private void UpdateForCreate(T entity)
        {
            if (entity is IEntityCreated)
            {
                if (((IEntityCreated)entity).Created == DateTime.MinValue)
                    ((IEntityCreated)entity).Created = DateTime.UtcNow;
            }

            if (entity is IEntity<Guid>)
            {
                ((IEntity<Guid>)entity).Id = Guid.NewGuid();
            }
        }

        private void UpdateForModified(T entity)
        {
            if (entity is IEntityModified)
                ((IEntityModified)entity).Modified = DateTime.UtcNow;
        }
    }
}
