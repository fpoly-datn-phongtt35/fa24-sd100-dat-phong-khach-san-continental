using Domain.Enums;

namespace Domain.DTO.Amenity;

public class AmenityUpdateRequest
{
    public Guid Id { get; set; }
    public string? Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public EntityStatus Status { get; set; }
    public DateTimeOffset? ModifiedTime { get; set; }
    public Guid? ModifiedBy { get; set; }

    public Models.Amenity ToAmenity()
    {
        return new Models.Amenity()
        {
            Id = Id,
            Name = Name,
            Description = Description,
            Status = Status,
            ModifiedTime = ModifiedTime,
            ModifiedBy = ModifiedBy
        };
    }
}