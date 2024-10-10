using Domain.DTO.Paging;
using Domain.Enums;

namespace Domain.DTO.PostType
{
    public class PostTypeGetRequest : PagingRequest
    {
        public string? TitleOfType { get; set; }
    }
}
