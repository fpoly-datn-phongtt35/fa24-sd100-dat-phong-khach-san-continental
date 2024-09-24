using Domain.Models;

namespace Domain.Repositories.IRepository;

public interface IAmenityRoomRepository
{
    Task<List<AmenityRoom>> GetAllAmenityRooms();
    Task<AmenityRoom?> GetAmenityRoomById(Guid amenityRoomId);
    Task<AmenityRoom> AddAmenityRoom(AmenityRoom amenityRoom);
    Task<AmenityRoom?> UpdateAmenityRoom(AmenityRoom amenityRoom);
    Task<AmenityRoom?> DeleteAmenityRoom(AmenityRoom amenityRoom);
}