using Microsoft.EntityFrameworkCore;
using System;

namespace Opw.EntityFrameworkCore
{
    public class DbMigrator<TDbContext> where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;

        public DbMigrator(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void CreateOrMigrate(Action<TDbContext> seedAction)
        {
            if (!_dbContext.Database.IsInMemory())
                _dbContext.Database.Migrate();

            seedAction?.Invoke(_dbContext);
        }
    }
}
