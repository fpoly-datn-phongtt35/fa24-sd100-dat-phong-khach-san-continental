using Domain.DTO.Amenity;
using Domain.Models;
using Domain.Repositories.IRepository;
using Domain.Services.IServices;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Domain.Services.Services;

public class AmenityService : IAmenityService
{
    private readonly IAmenityRepository _amenityRepository;
    private readonly IConfiguration _configuration;

    public AmenityService(IAmenityRepository amenityRepository, IConfiguration configuration)
    {
        _amenityRepository = amenityRepository;
        _configuration = configuration;
    }

    public async Task<AmenityResponse> AddAmenity(AmenityCreateRequest amenityCreateRequest)
    {
        // Create a new Amenity object from AmenityCreateRequest
        var newAmenity = new Amenity
        {
            Id = Guid.NewGuid(), // Generate a new GUID for the Amenity
            Name = amenityCreateRequest.Name,
            Description = amenityCreateRequest.Description,
            Status = amenityCreateRequest.Status,
            CreatedTime = amenityCreateRequest.CreatedTime ?? DateTimeOffset.UtcNow, // Use current time if not provided
            CreatedBy = amenityCreateRequest.CreatedBy
        };

        // Add the new Amenity using the repository
        var addedAmenity = await _amenityRepository.AddAmenity(newAmenity);

        // Convert the Amenity to AmenityResponse and return it
        var amenityResponse = AmenityResponse.AmenityExtensions.ToAmenityResponse(addedAmenity);

        return amenityResponse;
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