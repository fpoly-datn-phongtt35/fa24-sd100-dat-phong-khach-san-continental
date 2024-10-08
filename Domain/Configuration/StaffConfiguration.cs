﻿using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configuration
{
    public class StaffConfiguration : IEntityTypeConfiguration<Staff>
    {
        public void Configure(EntityTypeBuilder<Staff> builder)
        {
            builder.ToTable("Staff");
            builder.HasKey(t => t.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.UserName).IsUnicode(false).IsRequired();
            builder.HasIndex(x => x.UserName).IsUnique();
            builder.Property(x => x.Password).IsUnicode(false).IsRequired();
            builder.Property(x => x.FirstName).IsUnicode(true).IsRequired();
            builder.Property(x => x.LastName).IsUnicode(true).IsRequired();
            builder.Property(x => x.Email).IsUnicode(false).IsRequired();
            builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(x => x.PhoneNumber).IsUnicode(false).IsRequired().HasMaxLength(20);
            builder.HasOne(x => x.Role).WithMany(x => x.Staffs).HasForeignKey(x => x.RoleId);
            builder.HasMany(x => x.FeedBacks).WithOne(x => x.Staff).HasForeignKey(x => x.StaffId);
        }
    }
}
