using Domain.DTO.Amenity;
using Domain.Enums;
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
    
    public async Task<AmenityResponse?> GetAmenityById(Guid? amenityId)
    {
        if (amenityId == null)
            return null;
        
        var amenity = await _amenityRepository.GetAmenityById(amenityId.Value);
        if (amenity == null)
            return null;
        
        return amenity.ToAmenityResponse();
    }

    public async Task<List<AmenityResponse>> GetFilteredDeletedAmenities(string? searchString)
    {
        var deletedAmenities = await _amenityRepository.GetFilteredDeletedAmenity(searchString);
        var amenityResponses = deletedAmenities
            .Select(deletedAmenity => deletedAmenity.ToAmenityResponse())
            .ToList();

        return amenityResponses;
    }

    public async Task<List<AmenityResponse>> GetFilteredAmenities(EntityStatus? status, string? searchString)
    {
        var amenities = await _amenityRepository.GetFilteredAmenities(status!, searchString!);
        var amenityResponses = amenities
            .Select(a => a.ToAmenityResponse())
            .ToList();

        return amenityResponses;
    }
}