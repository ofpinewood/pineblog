using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opw.PineBlog.Resume.Entities;

namespace Opw.PineBlog.Resume.EntityFrameworkCore.Configuration
{
    internal class EducationEntityTypeConfiguration : IEntityTypeConfiguration<Education>
    {
        public void Configure(EntityTypeBuilder<Education> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.School).HasMaxLength(160).IsRequired();
            builder.Property(e => e.Degree).HasMaxLength(160);
            builder.Property(e => e.FieldOfStudy).HasMaxLength(160);
            builder.Property(e => e.Description).HasMaxLength(1000);
        }
    }
}
