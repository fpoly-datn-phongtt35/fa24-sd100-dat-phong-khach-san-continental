using System.Linq.Expressions;
using Domain.Enums;
using Domain.Models;

namespace Domain.Repositories.IRepository;

public interface IAmenityRoomRepository
{
    Task<List<AmenityRoom>> GetFilteredAmenityRooms(string? searchString, Guid? roomTypeId, EntityStatus? status);
    Task<AmenityRoom?> GetAmenityRoomById(Guid amenityRoomId);
    Task<AmenityRoom> AddAmenityRoom(AmenityRoom amenityRoom);
    Task<AmenityRoom?> UpdateAmenityRoom(AmenityRoom amenityRoom);
    Task<AmenityRoom?> DeleteAmenityRoom(AmenityRoom amenityRoom);
    Task<List<AmenityRoom>> GetFilteredDeletedAmenityRooms(string? searchString, Guid? roomTypeId);
    Task<AmenityRoom?> RecoverDeletedAmenityRoom(AmenityRoom amenityRoom);
}