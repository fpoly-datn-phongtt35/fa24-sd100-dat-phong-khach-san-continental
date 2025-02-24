using Domain.DTO.Building;
using Domain.DTO.Floor;
using Domain.DTO.Image;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.IRepository
{
    public interface IImagesRepository
    {
        Task<int> AddImages(ImagesCreateRequest request);
        Task<int> UpdateImages(ImagesUpdateRequest request);
        Task<int> DeleteImages(ImagesDeleteRequest request);
        Task<DataTable> GetImages(ImagesGetRequest Search);
        Task<DataTable> GetImagesById(Guid id);
    }
}
