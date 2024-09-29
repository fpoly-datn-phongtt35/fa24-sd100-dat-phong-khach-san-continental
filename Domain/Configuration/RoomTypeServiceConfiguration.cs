using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Configuration
{
    public class RoomTypeServiceConfiguration : IEntityTypeConfiguration<RoomTypeService>
    {
        public void Configure(EntityTypeBuilder<RoomTypeService> builder)
        {
            builder.ToTable("RoomTypeService");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Amount).IsRequired();
            builder.HasOne(x => x.RoomType).WithMany(x => x.RoomsTypeServices).HasForeignKey(x => x.RoomTypeId);
            builder.HasOne(x => x.Service).WithMany(x => x.RoomTypeServices).HasForeignKey(x => x.ServiceId);
        }
    }
}
