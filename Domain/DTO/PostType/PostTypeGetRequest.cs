using Domain.DTO.Paging;
using Domain.Enums;

namespace Domain.DTO.PostType
{
    public class PostTypeGetRequest : PagingRequest
    {
        public PostTypeEnum? TitleOfType { get; set; }
    }
}
