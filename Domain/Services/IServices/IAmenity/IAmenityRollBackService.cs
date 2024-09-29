using Domain.DTO.Amenity;

namespace Domain.Services.IServices.IAmenity;

public interface IAmenityRollBackService
{
    Task<AmenityResponse?> RollBackDeletedAmenity(AmenityUpdateRequest amenityUpdateRequest);
}