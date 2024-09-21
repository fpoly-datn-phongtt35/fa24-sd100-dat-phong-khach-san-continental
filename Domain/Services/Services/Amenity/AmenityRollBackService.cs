using Domain.DTO.Amenity;
using Domain.Enums;
using Domain.Repositories.IRepository;
using Domain.Services.IServices.IAmenity;

namespace Domain.Services.Services.Amenity;

public class AmenityRollBackService : IAmenityRollBackService
{
    private readonly IAmenityRepository _amenityRepository;

    public AmenityRollBackService(IAmenityRepository amenityRepository)
    {
        _amenityRepository = amenityRepository;
    }
    
    public async Task<AmenityResponse?> RollBackDeletedAmenity(AmenityUpdateRequest amenityUpdateRequest)
    {
        if (amenityUpdateRequest == null)
        {
            throw new ArgumentNullException(nameof(amenityUpdateRequest));
        }
        
        var existingAmenity = await _amenityRepository.GetAmenityById(amenityUpdateRequest.Id);
        
        if (existingAmenity == null)
        {
            throw new ArgumentException("Id amenity does not exist");
        }

        if (!existingAmenity.Deleted)
        {
            throw new InvalidOperationException("Amenity is not deleted, rollback not applicable.");
        }
        existingAmenity.Status = (EntityStatus)1;
        existingAmenity.ModifiedTime = amenityUpdateRequest.ModifiedTime;
        existingAmenity.ModifiedBy = amenityUpdateRequest.ModifiedBy;
        existingAmenity.Deleted = false;
        existingAmenity.DeletedTime = null;
        existingAmenity.DeletedBy = null;
        
        await _amenityRepository.RollBackDeletedAmenity(existingAmenity);
        
        return existingAmenity.ToAmenityResponse();
    }
}