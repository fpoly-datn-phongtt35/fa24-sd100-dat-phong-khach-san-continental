using Domain.DTO.Amenity;

namespace Domain.Services.IServices.IAmenity;

public interface IAmenityGetService
{
    Task<List<AmenityResponse>> GetAllAmenities(string? search);
    Task<AmenityResponse?> GetAmenityById(Guid? amenityId);
}