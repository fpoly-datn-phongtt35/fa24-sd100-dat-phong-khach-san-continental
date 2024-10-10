using Domain.DTO.Amenity;
using Domain.Enums;

namespace Domain.Services.IServices.IAmenity;

public interface IAmenityGetService
{
    Task<AmenityResponse?> GetAmenityById(Guid? amenityId);
    Task<List<AmenityResponse>> GetFilteredDeletedAmenities(string? searchString);
    Task<List<AmenityResponse>> GetFilteredAmenities(EntityStatus? status, string? searchString);
}