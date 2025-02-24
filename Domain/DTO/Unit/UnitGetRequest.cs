using Domain.DTO.Paging;
using Domain.Enums;

namespace Domain.DTO.Unit;

public class UnitGetRequest : PagingRequest
{
    public string? SearchString { get; set; }
    public EntityStatus? Status { get; set; }
}