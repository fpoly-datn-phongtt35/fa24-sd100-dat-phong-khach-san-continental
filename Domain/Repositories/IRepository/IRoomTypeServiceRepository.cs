using Domain.DTO.Paging;
using Domain.DTO.RoomTypeService;
using Domain.Enums;
using Domain.Models;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Domain.Repositories.IRepository;

public interface IRoomTypeServiceRepository
{
    Task<ResponseData<RoomTypeServiceResponse>> GetFilteredRoomTypeServices
        (RoomTypeServiceGetRequest roomTypeServiceGetRequest);
    Task<ResponseData<RoomTypeServiceResponse>> GetFilteredDeletedRoomTypeServices
        (RoomTypeServiceGetRequest roomTypeServiceGetRequest);
    Task<RoomTypeService?> GetRoomTypeServiceById(Guid roomTypeServiceId);
    Task<RoomTypeService> CreateRoomTypeService(RoomTypeService roomTypeService);
    Task<RoomTypeService?> UpdateRoomTypeService(RoomTypeService roomTypeService);
    Task<RoomTypeService?> DeleteRoomTypeService(RoomTypeService roomTypeService);
    Task<RoomTypeService?> RecoverDeletedRoomTypeService(RoomTypeService roomTypeService);
}