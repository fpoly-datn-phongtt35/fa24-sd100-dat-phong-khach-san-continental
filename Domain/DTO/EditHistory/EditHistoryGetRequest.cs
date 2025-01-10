using Domain.DTO.Paging;
using Domain.Enums;

namespace Domain.DTO.EditHistory;

public class EditHistoryGetRequest : PagingRequest
{
    public string? SearchString { get; set; }
    public For? For { get; set; }
}