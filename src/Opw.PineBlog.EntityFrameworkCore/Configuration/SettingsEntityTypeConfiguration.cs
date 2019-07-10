using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opw.PineBlog.Entities;

namespace Opw.PineBlog.EntityFrameworkCore.Configuration
{
    internal class SettingsEntityTypeConfiguration : IEntityTypeConfiguration<Settings>
    {
        public void Configure(EntityTypeBuilder<Settings> builder)
        {
            builder.HasKey(e => e.Created);

            builder.Property(e => e.Title).HasMaxLength(160).IsRequired();
            builder.Property(e => e.Description);
        }
    }
}
