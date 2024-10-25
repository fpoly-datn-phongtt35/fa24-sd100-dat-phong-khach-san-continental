using Domain.DTO.Amenity;
using Domain.DTO.Paging;
using Domain.Enums;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Domain.Services.IServices.IAmenity;

public interface IAmenityGetService
{
    Task<AmenityResponse?> GetAmenityById(Guid? amenityId);
    Task<ResponseData<AmenityResponse>> GetFilteredDeletedAmenities(AmenityGetRequest amenityGetRequest);
    Task<ResponseData<AmenityResponse>> GetFilteredAmenities(AmenityGetRequest amenityGetRequest);
}