using Domain.DTO.Amenity;
using Domain.Models;

namespace Domain.Services.IServices;

public interface IAmenityService
{
    Task<AmenityResponse> AddAmenity(AmenityCreateRequest amenityCreateRequest);
    Task<AmenityResponse> UpdateAmenity(AmenityUpdateRequest amenityUpdateRequest);
    Task<bool> DeleteAmenityById(Guid amenityId);
    Task<List<AmenityResponse>> GetAllAmenities();
    Task<AmenityResponse?> GetAmenityById(Guid amenityId);
}