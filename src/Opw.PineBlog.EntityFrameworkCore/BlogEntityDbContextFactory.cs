using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Opw.EntityFrameworkCore;
using System;

namespace Opw.PineBlog.EntityFrameworkCore
{
    [Obsolete("This class is needed to run \"dotnet ef...\" commands from command line on development. Do not use directly.")]
    public class BlogEntityDbContextFactory : DbContextFactory, IDesignTimeDbContextFactory<BlogEntityDbContext>
    {
        public BlogEntityDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<BlogEntityDbContext>();

            var connectionString = GetConfiguration().GetConnectionString("DefaultConnection");
            DbContextConfigurer.Configure(builder, connectionString);

            return new BlogEntityDbContext(builder.Options);
        }
    }
}
