using Domain.DTO.PostType;
using Domain.DTO.Paging;
using Domain.Models;
using Domain.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostTypeController : ControllerBase
    {
        private readonly IPostTypeService _postTypeRepo;
        public PostTypeController(IPostTypeService postTypeRepo)
        {
            _postTypeRepo = postTypeRepo;
        }

        [HttpPost("CreatePostType")]
        public async Task<int> CreatePostType(PostTypeCreateRequest request)
        {
            try
            {
                return await _postTypeRepo.AddPostType(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost("GetListPostType")]
        public async Task<ResponseData<PostType>> GetListPostType(PostTypeGetRequest request)
        {
            try
            {
                return await _postTypeRepo.GetAllPostType(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("GetPostTypeById")]
        public async Task<PostType> GetPostTypeById(Guid Id)
        {
            try
            {
                return await _postTypeRepo.GetPostTypeById(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("UpdatePostType")]
        public async Task<int> UpdatePostType(PostTypeUpdateRequest request)
        {
            try
            {
                return await _postTypeRepo.UpdatePostType(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("DeletePostType")]
        public async Task<int> DeletePostType(PostTypeDeleteRequest request)
        {
            try
            {
                return await _postTypeRepo.DeletePostType(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
