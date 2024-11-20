using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Microsoft.VisualBasic;

namespace Domain.DTO.RoomType;

public class RoomTypeUpdateRequest
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? MaximumOccupancy { get; set; }
    public EntityStatus Status { get; set; }
    public DateTimeOffset? ModifiedTime { get; set; }
    public Guid? ModifiedBy { get; set; }
    public bool Deleted { get; set; }

    public Models.RoomType ToRoomType()
    {
        return new Models.RoomType()
        {
            Id = Id,
            Name = Name,
            Description = Description,
            MaximumOccupancy = MaximumOccupancy,
            Status = Status,
            ModifiedTime = ModifiedTime,
            ModifiedBy = ModifiedBy,
            Deleted = Deleted
        };
    }
}