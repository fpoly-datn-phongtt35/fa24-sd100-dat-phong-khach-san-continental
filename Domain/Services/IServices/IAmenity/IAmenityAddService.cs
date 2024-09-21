using Domain.DTO.Amenity;

namespace Domain.Services.IServices.IAmenity;

public interface IAmenityAddService
{
    Task<AmenityResponse> AddAmenity(AmenityCreateRequest amenityCreateRequest);
}