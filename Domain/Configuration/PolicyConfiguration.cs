using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configuration
{
    public class PolicyConfiguration : IEntityTypeConfiguration<Policy>
    {
        public void Configure(EntityTypeBuilder<Policy> builder)
        {
            builder.ToTable("Policy");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Title).IsUnicode(true).IsRequired();
            builder.Property(x => x.Content).IsUnicode(true).IsRequired();
            builder.HasOne(x => x.PolicyType).WithMany(x => x.Policies).HasForeignKey(x => x.PolicyTypeId);
            builder.HasOne(x => x.Staff).WithMany(x => x.Policies).HasForeignKey(x => x.StaffId);
        }
    }
}
