using Domain.DTO.Paging;
using Domain.Enums;

namespace Domain.DTO.RoomType;

public class RoomTypeGetRequest : PagingRequest
{
    public string? SearchString { get; set; }
    public EntityStatus? Status { get; set; }
}