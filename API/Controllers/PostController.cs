using Domain.DTO.Paging;
using Domain.DTO.Post;
using Domain.Models;
using Domain.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postRepo;
        public PostController(IPostService postRepo)
        {
            _postRepo = postRepo;
        }

        [HttpPost("CreatePost")]
        public async Task<int> CreatePost(PostCreateRequest request)
        {
            try
            {
                return await _postRepo.AddPost(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost("GetListPost")]
        public async Task<ResponseData<Post>> GetListPost(PostGetRequest request)
        {
            try
            {
                return await _postRepo.GetAllPost(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("GetPostById")]
        public async Task<Post> GetPostById(Guid Id)
        {
            try
            {
                return await _postRepo.GetPostById(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("UpdatePost")]
        public async Task<int> UpdatePost(PostUpdateRequest request)
        {
            try
            {
                return await _postRepo.UpdatePost(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("DeletePost")]
        public async Task<int> DeletePost(PostDeleteRequest request)
        {
            try
            {
                return await _postRepo.DeletePost(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
