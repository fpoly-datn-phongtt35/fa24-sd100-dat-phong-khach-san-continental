using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configuration;

public class EditHistoryConfiguration : IEntityTypeConfiguration<EditHistory>
{
    public void Configure(EntityTypeBuilder<EditHistory> builder)
    {
        builder.ToTable("EditHistory");
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.RoomBookingDetail)
            .WithMany(x => x.EditHistory)
            .HasForeignKey(x => x.RoomBookingDetailId);
    }
}