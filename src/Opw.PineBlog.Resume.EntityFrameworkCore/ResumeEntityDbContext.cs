using Microsoft.EntityFrameworkCore;
using Opw.EntityFrameworkCore;

namespace Opw.PineBlog.Resume.EntityFrameworkCore
{
    public class ResumeEntityDbContext : EntityDbContext, IResumeEntityDbContext
    {
        //public DbSet<Author> Authors { get; set; }

        public ResumeEntityDbContext(DbContextOptions<ResumeEntityDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.SetTableName($"PineBlog_Resume_{entityType.GetTableName()}");
            }
        }
    }
}
