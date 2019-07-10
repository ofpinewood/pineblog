using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opw.PineBlog.Entities;

namespace Opw.PineBlog.EntityFrameworkCore.Configuration
{
    internal class AuthorEntityTypeConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasMany(e => e.Posts).WithOne(e => e.Author).HasForeignKey(e => e.AuthorId);

            builder.Property(e => e.UserId).IsRequired();
            builder.Property(e => e.UserName).HasMaxLength(160);
            builder.Property(e => e.DisplayName).HasMaxLength(160).IsRequired();
            builder.Property(e => e.Avatar).HasMaxLength(160);
            builder.Property(e => e.Email).HasMaxLength(254);
        }
    }
}
