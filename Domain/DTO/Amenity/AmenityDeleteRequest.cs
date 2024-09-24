using Domain.Enums;

namespace Domain.DTO.Amenity;

public class AmenityDeleteRequest
{
    public Guid Id { get; set; }
    public EntityStatus Status { get; set; }
    public bool Deleted { get; set; }
    public Guid? DeletedBy { get; set; }
    public DateTimeOffset? DeletedTime { get; set; }

    public Models.Amenity ToAmenity()
    {
        return new Models.Amenity()
        {
            Id = Id,
            Status = Status,
            Deleted = Deleted,
            DeletedBy = DeletedBy,
            DeletedTime = DeletedTime
        };
    }
}