using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configuration
{
    public class AmenityRoomDetailConfiguration : IEntityTypeConfiguration<AmenityRoomDetail>
    {
        public void Configure(EntityTypeBuilder<AmenityRoomDetail> builder)
        {
            builder.ToTable("AmenityRoomDetail");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Amount).IsRequired();
            builder.HasOne(x => x.Amenity).WithMany(x => x.AmenityRoomDetails).HasForeignKey(x => x.AmenityId);
            builder.HasOne(x => x.RoomType).WithMany(x => x.AmenityRoomDetails).HasForeignKey(x => x.RoomTypeId);
        }
    }
}
