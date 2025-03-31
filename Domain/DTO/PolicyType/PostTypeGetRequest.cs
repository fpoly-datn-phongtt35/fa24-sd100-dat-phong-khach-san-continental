using Domain.DTO.Paging;
using Domain.Enums;

namespace Domain.DTO.PolicyType
{
    public class PolicyTypeGetRequest : PagingRequest
    {
        public PolicyTypeEnum? TitleOfType { get; set; }
    }
}
