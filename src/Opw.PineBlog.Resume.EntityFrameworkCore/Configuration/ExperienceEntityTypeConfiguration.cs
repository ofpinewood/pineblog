using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opw.PineBlog.Resume.Entities;

namespace Opw.PineBlog.Resume.EntityFrameworkCore.Configuration
{
    internal class ExperienceEntityTypeConfiguration : IEntityTypeConfiguration<Experience>
    {
        public void Configure(EntityTypeBuilder<Experience> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Title).HasMaxLength(160).IsRequired();
            builder.Property(e => e.Description).HasMaxLength(1000);

            builder.Property(e => e.Company).HasMaxLength(160);
            builder.Property(e => e.CompanyUrl).HasMaxLength(254);

            builder.Property(e => e.Location).HasMaxLength(160);
        }
    }
}
