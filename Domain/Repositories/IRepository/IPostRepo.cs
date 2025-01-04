using Domain.DTO.Post;
using System.Data;

namespace Domain.Repositories.IRepository
{
    public interface IPostRepo
    {
        Task<int> AddPost(PostCreateRequest request);
        Task<int> UpdatePost(PostUpdateRequest request);
        Task<int> DeletePost(PostDeleteRequest request);
        Task<DataTable> GetAllPost(PostGetRequest request);
        Task<DataTable> GetPostById(Guid id);
        Task<List<PostTermsDto>> GetAllPostTerms();
    }
}
