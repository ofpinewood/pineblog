using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Opw.PineBlog.EntityFrameworkCore
{
    [ExcludeFromCodeCoverage]
    [Obsolete("This class is needed to run \"dotnet ef...\" commands from command line on development. Do not use directly.")]
    internal class BlogEntityDbContextFactory : IDesignTimeDbContextFactory<BlogEntityDbContext>
    {
        private const string DesignTimeConnectionString = "Server=.\\SQLEXPRESS; Database=pineblog-db; Trusted_Connection=True;";

        public BlogEntityDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<BlogEntityDbContext>();
            DbContextOptionsHelper.ConfigureOptionsBuilder(builder, DesignTimeConnectionString);
            return new BlogEntityDbContext(builder.Options);
        }
    }
}
