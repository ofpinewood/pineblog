using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opw.PineBlog.Resume.Entities;

namespace Opw.PineBlog.Resume.EntityFrameworkCore.Configuration
{
    internal class LinkEntityTypeConfiguration : IEntityTypeConfiguration<Link>
    {
        public void Configure(EntityTypeBuilder<Link> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Title).HasMaxLength(160);
            builder.Property(e => e.Url).HasMaxLength(254);
        }
    }
}
