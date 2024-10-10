using Domain.DTO.Paging;

namespace Domain.DTO.Customer
{
    public class CustomerGetRequest : PagingRequest
    {
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}
