using Domain.DTO.Image;
using Domain.DTO.Paging;
using Domain.Models;
using Domain.Services.IServices;
using Domain.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImagesService _imagesService;
        public ImagesController(IImagesService serviceImages)
        {
            _imagesService = serviceImages;
        }

        [HttpPost("CreateImages")]
        public async Task<int> CreateImages(ImagesCreateRequest request)
        {
            try
            {
                return await _imagesService.AddImages(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("GetListImages")]
        public async Task<ResponseData<Images>> GetListImages(ImagesGetRequest request)
        {
            try
            {
                return await _imagesService.GetImages(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("GetImagesById")]
        public async Task<Images> GetImagesById(Guid Id)
        {
            try
            {
                return await _imagesService.GetImagesById(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("UpdateImages")]
        public async Task<int> UpdateImages(ImagesUpdateRequest request)
        {
            try
            {
                return await _imagesService.UpdateImages(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("DeleteImages")]
        public async Task<int> DeleteImages(ImagesDeleteRequest request)
        {
            try
            {
                return await _imagesService.DeleteImages(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
