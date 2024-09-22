using Domain.DTO.Amenity;
using Domain.DTO.AmenityRoom;

namespace Domain.Services.IServices.IAmenity;

public interface IAmenityAddService
{
    Task<AmenityResponse> AddAmenity(AmenityCreateRequest amenityCreateRequest);
}