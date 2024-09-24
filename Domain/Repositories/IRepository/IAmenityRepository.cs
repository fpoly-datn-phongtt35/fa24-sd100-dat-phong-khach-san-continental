using Domain.Models;

namespace Domain.Repositories.IRepository;

public interface IAmenityRepository
{
    Task<Amenity> AddAmenity(Amenity amenity);
    Task<Amenity?> UpdateAmenity(Amenity amenity);
    Task<Amenity?> DeleteAmenityById(Amenity amenity);
    Task<List<Amenity>> GetAllAmenities();
    Task<Amenity?> GetAmenityById(Guid amenityId);
    string GenerateToken();
    Task<Amenity?> RollBackDeletedAmenity(Amenity amenity);
}