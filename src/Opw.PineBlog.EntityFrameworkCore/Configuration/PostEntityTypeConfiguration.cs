using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opw.PineBlog.Entities;

namespace Opw.PineBlog.EntityFrameworkCore.Configuration
{
    internal class PostEntityTypeConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.Author).WithMany(e => e.Posts).HasForeignKey(e => e.AuthorId);
            
            builder.Property(e => e.Title).HasMaxLength(160).IsRequired();
            builder.Property(e => e.Slug).HasMaxLength(160).IsRequired();
            builder.Property(e => e.Description).HasMaxLength(450).IsRequired();
            builder.Property(e => e.Categories).HasMaxLength(2000);
            builder.Property(e => e.Content).IsRequired();
        }
    }
}
