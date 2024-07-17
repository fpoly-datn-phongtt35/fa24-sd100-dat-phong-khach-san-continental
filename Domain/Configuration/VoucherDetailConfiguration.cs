using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configuration
{
    public class VoucherDetailConfiguration : IEntityTypeConfiguration<VoucherDetail>
    {
        public void Configure(EntityTypeBuilder<VoucherDetail> builder)
        {
            builder.ToTable("VoucherDetail");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Code).IsUnicode(false).IsRequired();
            builder.Property(x => x.StartDate).IsRequired();
            builder.Property(x => x.EndDate).IsRequired();
            builder.HasOne(x => x.Bill).WithMany(x => x.VoucherDetails).HasForeignKey(x => x.BillId);
            builder.HasOne(x => x.Voucher).WithMany(x => x.VoucherDetails).HasForeignKey(x => x.VoucherId);
        }
    }
}
