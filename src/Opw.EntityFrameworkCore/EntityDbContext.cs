using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.EntityFrameworkCore
{
    /// <summary>
    /// A EntityDbContext (derived from DbContext) instance represents a session with the database and can be used to
    /// query and save instances of your entities. EntityDbContext is a combination of the Unit Of Work and Repository patterns.
    /// The EntityDbContext automatically sets the IEntityCreated and IEntityModified properties.
    /// </summary>
    public abstract class EntityDbContext : DbContext, IEntityDbContext
    {
        protected EntityDbContext(DbContextOptions options) : base(options) { }

        public new Result<int> SaveChanges()
        {
            UpdatedCreateDate();
            UpdatedModifiedDate();
            NotifyEntityChangeWatchers();

            try
            {
                return Result<int>.Success(base.SaveChanges());
            }
            catch (Exception ex)
            {
                return Result<int>.Fail(ex);
            }
        }

        public new Result<int> SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdatedCreateDate();
            UpdatedModifiedDate();
            NotifyEntityChangeWatchers();

            try
            {
                return Result<int>.Success(base.SaveChanges(acceptAllChangesOnSuccess));
            }
            catch (Exception ex)
            {
                return Result<int>.Fail(ex);
            }
        }

        public new async Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken)
        {
            UpdatedCreateDate();
            UpdatedModifiedDate();
            NotifyEntityChangeWatchers();

            try
            {
                var result = await base.SaveChangesAsync(cancellationToken);
                return Result<int>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<int>.Fail(ex);
            }
        }

        public new async Task<Result<int>> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken)
        {
            UpdatedCreateDate();
            UpdatedModifiedDate();
            NotifyEntityChangeWatchers();

            try
            {
                var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
                return Result<int>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<int>.Fail(ex);
            }
        }

        private void NotifyEntityChangeWatchers()
        {
            foreach(var entry in ChangeTracker.Entries()
                .Where(i => i.State == EntityState.Modified || i.State == EntityState.Added))
            {
                EntityChangeObserver.Instance.OnChanged(new EntityChangeEventArgs(entry));
            }
        }

        private void UpdatedCreateDate()
        {
            var entries = ChangeTracker.Entries()
                .Where(i => i.State == EntityState.Added && i.Entity is IEntityCreated);

            foreach (var entry in entries)
            {
                if (((IEntityCreated)entry.Entity).Created == DateTime.MinValue)
                    ((IEntityCreated)entry.Entity).Created = DateTime.UtcNow;
            }
        }

        private void UpdatedModifiedDate()
        {
            var entries = ChangeTracker.Entries()
                .Where(i => (i.State == EntityState.Modified || i.State == EntityState.Added) && i.Entity is IEntityModified);

            foreach (var entry in entries)
            {
                ((IEntityModified)entry.Entity).Modified = DateTime.UtcNow;
            }
        }
    }
}
