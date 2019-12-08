using Microsoft.EntityFrameworkCore;
using Opw.EntityFrameworkCore;
using Opw.PineBlog.Resume.Entities;

namespace Opw.PineBlog.Resume
{
    public interface IResumeEntityDbContext : IEntityDbContext
    {
        DbSet<Profile> Profiles { get; set; }
    }
}