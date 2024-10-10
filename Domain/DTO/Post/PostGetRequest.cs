using Domain.DTO.Paging;

namespace Domain.DTO.Post
{
    public class PostGetRequest : PagingRequest
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
    }
}
