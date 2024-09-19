using Domain.DTO.Amenity;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories.IRepository;
using Domain.Services.IServices;
using Microsoft.Data.SqlClient;
using Utilities.StoredProcedure;

namespace Domain.Services.Services;

public class AmenityService : IAmenityService
{
    private readonly IAmenityRepository _amenityRepository;

    public AmenityService(IAmenityRepository amenityRepository)
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

    public Task<AmenityResponse> UpdateAmenity(AmenityUpdateRequest amenityUpdateRequest)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAmenityById(Guid amenityId)
    {
        throw new NotImplementedException();
    }

    public Task<List<AmenityResponse>> GetAllAmenities()
    {
       throw new NotImplementedException();
    }

    public Task<AmenityResponse?> GetAmenityById(Guid amenityId)
    {
        throw new NotImplementedException();
    }
}