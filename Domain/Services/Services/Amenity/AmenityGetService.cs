using System.Data;
using Domain.DTO.Amenity;
using Domain.DTO.Paging;
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

    public async Task<ResponseData<AmenityResponse>> GetFilteredDeletedAmenities(AmenityGetRequest amenityGetRequest)
    {
        return await _amenityRepository.GetFilteredDeletedAmenity(amenityGetRequest);
    }

    public async Task<ResponseData<AmenityResponse>> GetFilteredAmenities(AmenityGetRequest amenityGetRequest)
    {
        // ResponseData<AmenityResponse> model;
        // try
        // {
        //     model = await _amenityRepository.GetFilteredAmenities(amenityGetRequest);
        // }
        // catch (Exception e)
        // {
        //     throw new Exception("An error occurred while retrieving paginated amenities", e);
        // }
        // return model;
        return await _amenityRepository.GetFilteredAmenities(amenityGetRequest);
    }
}