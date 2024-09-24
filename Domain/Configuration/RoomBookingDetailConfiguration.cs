using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configuration
{
    public class RoomBookingDetailConfiguration : IEntityTypeConfiguration<RoomBookingDetail>
    {
        public void Configure(EntityTypeBuilder<RoomBookingDetail> builder)
        {
            builder.ToTable("RoomBookingDetail");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.CheckInBooking).IsRequired();
            builder.Property(x => x.CheckOutBooking).IsRequired();
            builder.Property(x => x.CheckInReality).IsRequired();
            builder.Property(x => x.CheckOutReality).IsRequired();
            builder.Property(x => x.Deposit).HasDefaultValue(0);
            builder.HasOne(x => x.Room).WithMany(x => x.RoomBookingDetails).HasForeignKey(x => x.RoomId);
            builder.HasOne(x => x.RoomBooking).WithMany(x => x.RoomBookingDetails).HasForeignKey(x => x.RoomBookingId);
        }
    }
}
