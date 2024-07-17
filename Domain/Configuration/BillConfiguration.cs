using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configuration
{
    public class BillConfiguration : IEntityTypeConfiguration<Bill>
    {
        public void Configure(EntityTypeBuilder<Bill> builder)
        {
            builder.ToTable("Bill");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(x => x.Customer).WithMany(x => x.Bills).HasForeignKey(x => x.CustomerId);
            builder.HasOne(x => x.RoomBooking).WithOne().HasForeignKey<Bill>(x => x.RoomBookingId).IsRequired(false);
            builder.HasOne(x => x.ServiceOrder).WithOne().HasForeignKey<Bill>(x => x.ServiceOrderId).IsRequired(false);
            builder.HasOne(x => x.FeedBack).WithOne().HasForeignKey<Bill>(x => x.FeedBackId).IsRequired(false);
        }
    }
}
