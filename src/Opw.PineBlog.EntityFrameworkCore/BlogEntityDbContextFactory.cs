using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Opw.EntityFrameworkCore;
using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Opw.PineBlog.EntityFrameworkCore
{
    [Obsolete("This class is needed to run \"dotnet ef...\" commands from command line on development. Do not use directly.")]
    public class BlogEntityDbContextFactory : IDesignTimeDbContextFactory<BlogEntityDbContext>
    {
        public BlogEntityDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<BlogEntityDbContext>();

            var connectionString = GetConfiguration().GetConnectionString("DefaultConnection");
            DbContextOptionsHelper.ConfigureOptionsBuilder(builder, connectionString);

            return new BlogEntityDbContext(builder.Options);
        }

        private IConfiguration GetConfiguration()
        {
            var projectDir = GetApplicationRoot();
            var builder = new ConfigurationBuilder()
                .SetBasePath(projectDir)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            return builder.Build();
        }

        private string GetApplicationRoot()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            var regex = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = regex.Match(path).Value;
            return appRoot;
        }
    }
}
