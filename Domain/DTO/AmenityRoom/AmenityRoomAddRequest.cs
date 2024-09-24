using Domain.Enums;

namespace Domain.DTO.AmenityRoom;

public class AmenityRoomAddRequest
{
    public Guid AmenityId { get; set; }
    public Guid RoomTypeId { get; set; }
    public int Amount { get; set; }
    public EntityStatus Status { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    public Guid? CreatedBy { get; set; }

    /// <summary>
    /// Convert the current object of AmenityRoomRequest into a
    /// new object of AmenityRoom type
    /// </summary>
    /// <returns>Return AmenityRoom object</returns>
    public Models.AmenityRoom ToAmenityRoom()
    {
        return new Models.AmenityRoom()
        {
            AmenityId = AmenityId,
            RoomTypeId = RoomTypeId,
            Amount = Amount,
            Status = Status,
            CreatedTime = CreatedTime,
            CreatedBy = CreatedBy
        };
    }
}