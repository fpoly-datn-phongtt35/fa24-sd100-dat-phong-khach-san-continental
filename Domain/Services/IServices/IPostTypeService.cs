using Domain.DTO.PostType;
using Domain.DTO.Paging;
using Domain.Models;

namespace Domain.Services.IServices
{
    public interface IPostTypeService
    {
        Task<int> AddPostType(PostTypeCreateRequest request);
        Task<int> UpdatePostType(PostTypeUpdateRequest request);
        Task<int> DeletePostType(PostTypeDeleteRequest request);
        Task<ResponseData<PostType>> GetAllPostType(PostTypeGetRequest PostType);
        Task<PostType> GetPostTypeById(Guid Id);
    }
}
