using Domain.Enums;

namespace Domain.DTO.Amenity;

public class AmenityCreateRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public EntityStatus Status { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    public Guid? CreatedBy { get; set; }
}