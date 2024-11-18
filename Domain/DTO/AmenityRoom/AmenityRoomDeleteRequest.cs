using Domain.Enums;

namespace Domain.DTO.AmenityRoom;

public class AmenityRoomDeleteRequest
{
    public Guid Id { get; set; }
    public EntityStatus Status { get; set; }
    public bool Deleted { get; set; }
    public DateTimeOffset? DeletedTime { get; set; }
    public Guid? DeletedBy { get; set; }

    public Models.AmenityRoom ToAmenityRoom()
    {
        return new Models.AmenityRoom()
        {
            Id = Id,
            Status = Status,
            Deleted = Deleted,
            DeletedTime = DeletedTime,
            DeletedBy = DeletedBy
        };
    }
}