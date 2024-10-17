using System.Data;
using Domain.DTO.Amenity;
using Domain.DTO.Paging;
using Domain.Enums;
using Domain.Models;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Domain.Repositories.IRepository;

public interface IAmenityRepository
{
    Task<Amenity> AddAmenity(Amenity amenity);
    Task<Amenity?> UpdateAmenity(Amenity amenity);
    Task<Amenity?> DeleteAmenityById(Amenity amenity);
    Task<Amenity?> GetAmenityById(Guid amenityId);
    Task<Amenity?> RecoverDeletedAmenity(Amenity amenity);
    Task<ResponseData<AmenityResponse>> GetFilteredDeletedAmenity(AmenityGetRequest amenityGetRequest);
    Task<ResponseData<AmenityResponse>> GetFilteredAmenities(AmenityGetRequest amenityGetRequest);
}