using Domain.DTO.PostType;
using System.Data;

namespace Domain.Repositories.IRepository
{
    public interface IPostTypeRepo
    {
        Task<int> AddPostType(PostTypeCreateRequest request);
        Task<int> UpdatePostType(PostTypeUpdateRequest request);
        Task<int> DeletePostType(PostTypeDeleteRequest request);
        Task<DataTable> GetAllPostType(PostTypeGetRequest PostType);
        Task<DataTable> GetPostTypeById(Guid id);
    }
}
