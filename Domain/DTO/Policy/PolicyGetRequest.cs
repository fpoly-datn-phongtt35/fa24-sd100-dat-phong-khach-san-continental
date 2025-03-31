using Domain.DTO.Paging;

namespace Domain.DTO.Policy
{
    public class PolicyGetRequest : PagingRequest
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
    }
}
