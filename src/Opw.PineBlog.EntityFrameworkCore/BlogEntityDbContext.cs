using Microsoft.EntityFrameworkCore;
using Opw.PineBlog.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace Opw.PineBlog.EntityFrameworkCore
{
    public class BlogEntityDbContext : DbContext
    {
        public DbSet<BlogSettings> BlogSettings { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Author> Authors { get; set; }

        public BlogEntityDbContext(DbContextOptions<BlogEntityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.SetTableName($"PineBlog_{entityType.GetTableName()}");
            }
        }

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
            foreach (var entry in ChangeTracker.Entries()
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
