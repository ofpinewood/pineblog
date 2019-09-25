using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opw.PineBlog.Entities;

namespace Opw.PineBlog.EntityFrameworkCore.Configuration
{
    internal class BlogSettingsEntityTypeConfiguration : IEntityTypeConfiguration<BlogSettings>
    {
        public void Configure(EntityTypeBuilder<BlogSettings> builder)
        {
            builder.HasKey(e => e.Created);

            builder.Property(e => e.Title).HasMaxLength(160).IsRequired();
            builder.Property(e => e.Description).HasMaxLength(450);
            builder.Property(e => e.CoverUrl).HasMaxLength(254);
            builder.Property(e => e.CoverCaption).HasMaxLength(160);
            builder.Property(e => e.CoverLink).HasMaxLength(254);
        }
    }
}
