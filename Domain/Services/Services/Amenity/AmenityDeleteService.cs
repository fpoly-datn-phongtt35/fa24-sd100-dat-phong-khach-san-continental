using Domain.DTO.Amenity;
using Domain.Enums;
using Domain.Repositories.IRepository;
using Domain.Services.IServices.IAmenity;

namespace Domain.Services.Services.Amenity;

public class AmenityDeleteService : IAmenityDeleteService
{
    private readonly IAmenityRepository _amenityRepository;

    public AmenityDeleteService(IAmenityRepository amenityRepository)
    {
        _amenityRepository = amenityRepository;
    }
    
    public async Task<AmenityResponse?> DeleteAmenityById(AmenityDeleteRequest amenityDeleteRequest)
    {
        if (amenityDeleteRequest == null)
        {
            throw new ArgumentNullException(nameof(amenityDeleteRequest));
        }
        var existingAmenity = await _amenityRepository.GetAmenityById(amenityDeleteRequest.Id);
        if (existingAmenity == null)
        {
            throw new ArgumentException("Id amenity does not exist");
        }

        existingAmenity.Status = (EntityStatus)3;
        existingAmenity.Deleted = true;
        existingAmenity.DeletedTime = amenityDeleteRequest.DeletedTime;
        existingAmenity.DeletedBy = amenityDeleteRequest.DeletedBy;
        
        await _amenityRepository.DeleteAmenityById(existingAmenity);
        
        return existingAmenity.ToAmenityResponse();
    }
}