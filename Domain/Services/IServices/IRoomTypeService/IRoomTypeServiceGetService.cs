using Domain.DTO.Paging;
using Domain.DTO.RoomTypeService;
using Domain.Enums;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Domain.Services.IServices.IRoomTypeService;

public interface IRoomTypeServiceGetService
{
    Task<ResponseData<RoomTypeServiceResponse>> GetFilteredRoomTypeServices
        (RoomTypeServiceGetRequest roomTypeServiceGetRequest);
    Task<ResponseData<RoomTypeServiceResponse>>  GetFilteredDeletedRoomTypeServices
        (RoomTypeServiceGetRequest roomTypeServiceGetRequest);
    Task<RoomTypeServiceResponse?> GetRoomTypeServiceById(Guid? roomTypeServiceId);
}