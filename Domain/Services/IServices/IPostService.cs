using Domain.DTO.Paging;
using Domain.DTO.Post;
using Domain.Models;

namespace Domain.Services.IServices
{
    public interface IPostService
    {
        Task<int> AddPost(PostCreateRequest request);
        Task<int> UpdatePost(PostUpdateRequest request);
        Task<int> DeletePost(PostDeleteRequest request);
        Task<ResponseData<Post>> GetAllPost(PostGetRequest Post);
        Task<Post> GetPostById(Guid Id);
    }
}
