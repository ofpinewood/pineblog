using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opw.PineBlog.Resume.Entities;

namespace Opw.PineBlog.Resume.EntityFrameworkCore.Configuration
{
    internal class ProfileEntityTypeConfiguration : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.Property(e => e.UserName).HasMaxLength(160).IsRequired();
            builder.HasKey(e => e.UserName);

            builder.Property(e => e.Slug).HasMaxLength(160).IsRequired();

            builder.Property(e => e.FirstName).HasMaxLength(64);
            builder.Property(e => e.LastName).HasMaxLength(128);
            builder.Property(e => e.Email).HasMaxLength(254);

            builder.Property(e => e.Headline).HasMaxLength(160);
            builder.Property(e => e.Summary).HasMaxLength(1000);
            builder.Property(e => e.Industry).HasMaxLength(160);

            builder.Property(e => e.Country).HasMaxLength(128);
            builder.Property(e => e.Region).HasMaxLength(128);
        }
    }
}
