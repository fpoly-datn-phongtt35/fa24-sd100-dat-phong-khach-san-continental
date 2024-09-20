using Domain.DTO.Amenity;

namespace Domain.Services.IServices;

public interface IAmenityService
{
    Task<AmenityResponse> AddAmenity(AmenityCreateRequest amenityCreateRequest);
    Task<AmenityResponse?> UpdateAmenity(AmenityUpdateRequest amenityUpdateRequest);
    Task<AmenityResponse?> DeleteAmenityById(AmenityDeleteRequest amenityDeleteRequest);
    Task<List<AmenityResponse>> GetAllAmenities();
    Task<AmenityResponse?> GetAmenityById(Guid? amenityId);
}