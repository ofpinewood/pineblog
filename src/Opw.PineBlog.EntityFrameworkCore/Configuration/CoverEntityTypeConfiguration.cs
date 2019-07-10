using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opw.PineBlog.Entities;

namespace Opw.PineBlog.EntityFrameworkCore.Configuration
{
    internal class CoverEntityTypeConfiguration : IEntityTypeConfiguration<Cover>
    {
        public void Configure(EntityTypeBuilder<Cover> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Url).HasMaxLength(254);
            builder.Property(e => e.Caption).HasMaxLength(160);
            builder.Property(e => e.Link).HasMaxLength(254);
        }
    }
}
