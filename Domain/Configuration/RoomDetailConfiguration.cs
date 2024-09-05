﻿using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Domain.Configuration
{
    public class RoomDetailConfiguration : IEntityTypeConfiguration<RoomDetail>
    {
        public void Configure(EntityTypeBuilder<RoomDetail> builder)
        {
            builder.ToTable("RoomDetail");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsUnicode(false).IsRequired();
            builder.Property(x => x.Address).IsUnicode(false).IsRequired();
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.Description).IsUnicode(true).IsRequired();
            builder.Property(x => x.RoomSize).IsRequired();
            builder.Property(x => x.Images).HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<List<string>>(v));
            builder.HasOne(x => x.Floor).WithMany(x => x.RoomDetails).HasForeignKey(x => x.FloorId);
            builder.HasOne(x => x.RoomType).WithMany(x => x.RoomDetails).HasForeignKey(x => x.RoomTypeId);
        }
    }
}