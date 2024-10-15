using Domain.DTO.Paging;
using Domain.Enums;

namespace Domain.DTO.AmenityRoom;

public class AmenityRoomGetRequest : PagingRequest
{
    public string? SearchString { get; set; }
    public EntityStatus? Status { get; set; }
    public Guid? RoomTypeId { get; set; }
}