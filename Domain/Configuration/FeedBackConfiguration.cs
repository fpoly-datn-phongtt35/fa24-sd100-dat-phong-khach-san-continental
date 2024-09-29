using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configuration
{
    public class FeedBackConfiguration : IEntityTypeConfiguration<FeedBack>
    {
        public void Configure(EntityTypeBuilder<FeedBack> builder)
        {
            builder.ToTable("FeedBack");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Comments).IsUnicode(true).IsRequired();
            builder.Property(x => x.Rating).IsRequired();
            builder.HasOne(x => x.Staff)
                 .WithMany(x => x.FeedBacks)
                 .HasForeignKey(x => x.StaffId)
                 .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Customer)
                .WithMany(x => x.FeedBacks)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.RoomBooking)
                .WithMany(x => x.FeedBacks)
                .HasForeignKey(x => x.RoomBookingId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
