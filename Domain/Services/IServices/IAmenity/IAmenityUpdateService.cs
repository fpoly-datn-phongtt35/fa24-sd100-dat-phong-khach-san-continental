using Domain.DTO.Amenity;

namespace Domain.Services.IServices.IAmenity;

public interface IAmenityUpdateService
{
    Task<AmenityResponse?> UpdateAmenity(AmenityUpdateRequest amenityUpdateRequest);
    Task<AmenityResponse?> RecoverDeletedAmenity(AmenityUpdateRequest amenityUpdateRequest);
}