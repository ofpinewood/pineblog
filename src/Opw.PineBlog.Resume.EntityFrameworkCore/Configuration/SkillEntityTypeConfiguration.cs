using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opw.PineBlog.Resume.Entities;

namespace Opw.PineBlog.Resume.EntityFrameworkCore.Configuration
{
    internal class SkillEntityTypeConfiguration : IEntityTypeConfiguration<Skill>
    {
        public void Configure(EntityTypeBuilder<Skill> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name).HasMaxLength(64);
            builder.Property(e => e.Description).HasMaxLength(450);
        }
    }
}
