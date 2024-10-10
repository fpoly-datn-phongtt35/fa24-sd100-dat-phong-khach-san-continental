using Domain.Enums;

namespace Domain.DTO.Amenity;

public class AmenityGetRequest
{
    public string? SearchString { get; set; }
    public string? SearchBy { get; set; }
    public EntityStatus Status { get; set; }
}