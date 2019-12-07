using Microsoft.EntityFrameworkCore;
using Opw.EntityFrameworkCore;

namespace Opw.PineBlog.Resume
{
    public interface IResumeEntityDbContext : IEntityDbContext
    {
        //DbSet<Post> Posts { get; set; }
    }
}