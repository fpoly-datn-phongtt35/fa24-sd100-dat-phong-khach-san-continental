using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configuration
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Post");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Title).IsUnicode(true).IsRequired();
            builder.Property(x => x.Content).IsUnicode(true).IsRequired();
            builder.HasOne(x => x.PostType).WithMany(x => x.Posts).HasForeignKey(x => x.PostTypeId);
            builder.HasOne(x => x.Staff).WithMany(x => x.Posts).HasForeignKey(x => x.StaffId);
        }
    }
}
