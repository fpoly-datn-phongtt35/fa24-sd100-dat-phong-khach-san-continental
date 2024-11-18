using Domain.DTO.Paging;

namespace Domain.DTO.Role
{
    public class RoleGetRequest : PagingRequest
    {
        public string? Name { get; set; } = string.Empty;
    }
}
