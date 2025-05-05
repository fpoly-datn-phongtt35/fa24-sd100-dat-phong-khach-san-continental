using Domain.DTO.Paging;
using Domain.Enums;

namespace Domain.DTO.PolicyType
{
    public class PolicyTypeGetRequest : PagingRequest
    {
        public string? TitleOfType { get; set; }
        public EntityStatus? Status { get; set; }
    }
}
