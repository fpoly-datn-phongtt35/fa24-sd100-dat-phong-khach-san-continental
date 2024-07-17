using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configuration
{
    public class RoomBookingConfiguration : IEntityTypeConfiguration<RoomBooking>
    {
        public void Configure(EntityTypeBuilder<RoomBooking> builder)
        {
            builder.ToTable("RoomBooking");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.BookingType).IsRequired();
            builder.HasOne(x => x.Customer).WithMany(x => x.RoomBookings).HasForeignKey(x => x.CustomerId);
            builder.HasOne(x => x.User).WithMany(x => x.RoomBookings).HasForeignKey(x => x.UserId).IsRequired(false);
        }
    }
}
