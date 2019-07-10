using Microsoft.EntityFrameworkCore;
using Opw.PineBlog.Entities;
using Opw.EntityFrameworkCore;

namespace Opw.PineBlog
{
    public interface IBlogEntityDbContext : IEntityDbContext
    {
        DbSet<Settings> Settings { get; set; }
        DbSet<Author> Authors { get; set; }
        DbSet<Post> Posts { get; set; }
        DbSet<Cover> Covers { get; set; }
    }
}