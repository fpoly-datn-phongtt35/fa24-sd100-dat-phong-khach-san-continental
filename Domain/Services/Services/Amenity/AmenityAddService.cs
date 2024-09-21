using Domain.DTO.Amenity;
using Domain.Repositories.IRepository;
using Domain.Services.IServices.IAmenity;

namespace Domain.Services.Services.Amenity;

public class AmenityAddService : IAmenityAddService
{
    private readonly IAmenityRepository _amenityRepository;

    public AmenityAddService(IAmenityRepository amenityRepository)
    {
        _amenityRepository = amenityRepository;
    }
    
    public async Task<AmenityResponse> AddAmenity(AmenityCreateRequest amenityCreateRequest)
    {
        // Check if AmenityCreateRequest is not null
        if (amenityCreateRequest == null)
        {
            throw new ArgumentNullException(nameof(amenityCreateRequest));
        }
        // Convert amenityCreateRequest into Amenity type
        var amenity = amenityCreateRequest.ToAmenity();
        
        amenity.Id = Guid.NewGuid();
        
        // Add amenity object to AmenityResponse type
        await _amenityRepository.AddAmenity(amenity);

        return amenity.ToAmenityResponse();
    }
}