using Domain.Enums;
using Domain.Models;

namespace Domain.Repositories.IRepository;

public interface IAmenityRepository
{
    Task<Amenity> AddAmenity(Amenity amenity);
    Task<Amenity?> UpdateAmenity(Amenity amenity);
    Task<Amenity?> DeleteAmenityById(Amenity amenity);
    Task<Amenity?> GetAmenityById(Guid amenityId);
    Task<List<Amenity>> GetFilteredDeletedAmenity(string? searchString);
    Task<Amenity?> RecoverDeletedAmenity(Amenity amenity);
    Task<List<Amenity>> GetFilteredAmenities(EntityStatus? status, string? searchString);
    string GenerateToken();
}