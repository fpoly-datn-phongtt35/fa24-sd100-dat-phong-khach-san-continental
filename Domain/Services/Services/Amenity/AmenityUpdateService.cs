using Domain.DTO.Amenity;
using Domain.Repositories.IRepository;
using Domain.Services.IServices.IAmenity;

namespace Domain.Services.Services.Amenity;

public class AmenityUpdateService : IAmenityUpdateService
{
    private readonly IAmenityRepository _amenityRepository;

    public AmenityUpdateService(IAmenityRepository amenityRepository)
    {
        _amenityRepository = amenityRepository;
    }
    
    public async Task<AmenityResponse?> UpdateAmenity(AmenityUpdateRequest amenityUpdateRequest)
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

        if (existingAmenity.Deleted)
        {
            throw new InvalidOperationException("This amenity already deleted, cannot edit amenity");
        }
        existingAmenity.Name = amenityUpdateRequest.Name;
        existingAmenity.Description = amenityUpdateRequest.Description;
        existingAmenity.Status = amenityUpdateRequest.Status;
        existingAmenity.ModifiedTime = amenityUpdateRequest.ModifiedTime;
        existingAmenity.ModifiedBy = amenityUpdateRequest.ModifiedBy;
        
        await _amenityRepository.UpdateAmenity(existingAmenity);
        
        return existingAmenity.ToAmenityResponse();
    }
}