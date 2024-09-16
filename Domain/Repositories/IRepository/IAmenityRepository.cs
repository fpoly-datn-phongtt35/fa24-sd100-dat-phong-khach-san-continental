using Domain.Models;

namespace Domain.Repositories.IRepository;

public interface IAmenityRepository
{
    Task<Amenity> AddAmenity(Amenity amenity);
    Task<Amenity> UpdateAmenity(Amenity amenity);
    Task<bool> DeleteAmenityById(Guid amenityId);
    Task<List<Amenity>> GetAllAmenities();
    Task<Amenity?> GetAmenityById(Guid amenityId);
}