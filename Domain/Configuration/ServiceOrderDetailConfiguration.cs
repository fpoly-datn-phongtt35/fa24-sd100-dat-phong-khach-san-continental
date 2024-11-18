using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configuration
{
    public class ServiceOrderDetailConfiguration : IEntityTypeConfiguration<ServiceOrderDetail>
    {
        public void Configure(EntityTypeBuilder<ServiceOrderDetail> builder)
        {
            builder.ToTable("ServiceOrderDetail");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.Amount).IsRequired();
            builder.HasOne(x => x.Service).WithMany(x => x.ServiceOrderDetails).HasForeignKey(x => x.ServiceId);
            builder.HasOne(x => x.RoomBooking).WithMany(x => x.ServiceOrderDetails).HasForeignKey(x => x.RoomBookingId);
        }
    }
}
