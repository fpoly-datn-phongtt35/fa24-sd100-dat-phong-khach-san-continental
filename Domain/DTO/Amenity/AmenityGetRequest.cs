using Domain.DTO.Paging;
using Domain.Enums;

namespace Domain.DTO.Amenity;

public class AmenityGetRequest : PagingRequest
{
    public string? SearchString { get; set; }
    public EntityStatus? Status { get; set; }
}