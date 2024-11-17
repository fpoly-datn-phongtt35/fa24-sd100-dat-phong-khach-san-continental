using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configuration
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customer");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.UserName).IsUnicode(true).IsRequired();
            builder.Property(x => x.Password).IsUnicode(false).IsRequired();
            builder.Property(x => x.FirstName).IsUnicode(true);
            builder.Property(x => x.LastName).IsUnicode(true);
            builder.Property(x => x.Email).IsUnicode(false).IsRequired();
            builder.Property(x => x.PhoneNumber).IsUnicode(false).IsRequired().HasMaxLength(20);
            builder.HasMany(x => x.FeedBacks).WithOne(x => x.Customer).HasForeignKey(x => x.CustomerId);
        }
    }
}
