using Domain.DTO.Building;
using Domain.DTO.Image;
using Domain.DTO.Paging;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.IServices
{
    public interface IImagesService
    {
        Task<int> AddImages(ImagesCreateRequest request);
        Task<int> UpdateImages(ImagesUpdateRequest request);
        Task<int> DeleteImages(ImagesDeleteRequest request);
        Task<Images> GetImagesById(Guid Id);
        Task<ResponseData<Images>> GetImages(ImagesGetRequest Search);
    }
}
