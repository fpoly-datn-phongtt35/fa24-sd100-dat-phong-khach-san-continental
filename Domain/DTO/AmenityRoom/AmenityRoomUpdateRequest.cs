using Domain.Enums;

namespace Domain.DTO.AmenityRoom;

public class AmenityRoomUpdateRequest
{
    public Guid Id { get; set; }
    public Guid AmenityId { get; set; }
    public Guid RoomTypeId { get; set; }
    public int Amount { get; set; }
    public EntityStatus Status { get; set; }
    public DateTimeOffset ModifiedTime { get; set; }
    public Guid? ModifiedBy { get; set; }

    public Models.AmenityRoom ToAmenityRoom()
    {
        return new Models.AmenityRoom()
        {
            Id = Id,
            AmenityId = AmenityId,
            RoomTypeId = RoomTypeId,
            Amount = Amount,
            Status = Status,
            ModifiedTime = ModifiedTime,
            ModifiedBy = ModifiedBy
        };
    }
}