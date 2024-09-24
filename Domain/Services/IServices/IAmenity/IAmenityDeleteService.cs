using Domain.DTO.Amenity;

namespace Domain.Services.IServices.IAmenity;

public interface IAmenityDeleteService
{
    Task<AmenityResponse?> DeleteAmenityById(AmenityDeleteRequest amenityDeleteRequest);
}