using Domain.DTO.Paging;

namespace Domain.DTO.Customer
{
    public class CustomerGetByUserNameRequest : PagingRequest
    {
        public string UserName { get; set; } = string.Empty;
    }
}
