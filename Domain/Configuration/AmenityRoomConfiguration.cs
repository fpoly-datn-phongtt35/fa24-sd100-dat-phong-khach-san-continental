using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configuration
{
    public class AmenityRoomConfiguration : IEntityTypeConfiguration<AmenityRoom>
    {
        public void Configure(EntityTypeBuilder<AmenityRoom> builder)
        {
            builder.ToTable("AmenityRoom");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Amount).IsRequired();
            builder.HasOne(x => x.Amenity).WithMany(x => x.AmenityRooms).HasForeignKey(x => x.AmenityId);
            builder.HasOne(x => x.RoomType).WithMany(x => x.AmenityRooms).HasForeignKey(x => x.RoomTypeId);
        }
    }
}
