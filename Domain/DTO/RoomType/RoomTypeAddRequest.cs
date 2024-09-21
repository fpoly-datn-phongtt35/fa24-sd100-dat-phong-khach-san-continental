using Domain.Enums;

namespace Domain.DTO.RoomType;

public class RoomTypeAddRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int MaximumOccupancy { get; set; }
    public EntityStatus Status { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    public Guid? CreatedBy { get; set; }

    public Models.RoomType ToRoomType()
    {
        return new Models.RoomType()
        {
            Name = Name,
            Description = Description,
            MaximumOccupancy = MaximumOccupancy,
            Status = Status,
            CreatedTime = CreatedTime,
            CreatedBy = CreatedBy
        };
    }
}