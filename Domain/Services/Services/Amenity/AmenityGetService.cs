using Domain.DTO.Amenity;
using Domain.Repositories.IRepository;
using Domain.Services.IServices.IAmenity;

namespace Domain.Services.Services.Amenity;

public class AmenityGetService : IAmenityGetService
{
    private readonly IAmenityRepository _amenityRepository;

    public AmenityGetService(IAmenityRepository amenityRepository)
    {
        _amenityRepository = amenityRepository;
    }
    
    public async Task<List<AmenityResponse>> GetAllAmenities()
    {
        var amenities = await _amenityRepository.GetAllAmenities();

        var amenityResponses = amenities
            .Select(amenity => amenity.ToAmenityResponse())
            .ToList();
        
        return amenityResponses;
    }
    
    public async Task<AmenityResponse?> GetAmenityById(Guid? amenityId)
    {
        if (amenityId == null)
            return null;
        
        var amenity = await _amenityRepository.GetAmenityById(amenityId.Value);
        if (amenity == null)
            return null;
        
        return amenity.ToAmenityResponse();
    }
}