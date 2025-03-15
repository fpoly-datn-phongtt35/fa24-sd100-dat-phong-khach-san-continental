using Domain.DTO.Image;
using Domain.DTO.Paging;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories.Repository;
using Domain.Services.IServices;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Domain.Services.Services
{
    public class ImagesService : IImagesService
    {
        private readonly ImagesRepo _imagesRepo;
        private readonly IConfiguration _configuration;
        public ImagesService(IConfiguration configuration)
        {
            _configuration = configuration;
            _imagesRepo = new ImagesRepo(_configuration);
        }
        public Task<int> AddImages(ImagesCreateRequest request)
        {
            try
            {
                return _imagesRepo.AddImages(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<int> DeleteImages(ImagesDeleteRequest request)
        {
            try
            {
                return _imagesRepo.DeleteImages(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Images> GetImagesById(Guid Id)
        {
            Images Images = new();
            try
            {
                DataTable table = await _imagesRepo.GetImagesById(Id);
                Images = (from row in table.AsEnumerable()
                            select new Images
                            {
                                Id = row.Field<Guid>("Id"),
                                ObjId = row.IsNull("ObjId") ? (Guid?)null : row.Field<Guid>("ObjId"),
                                Image = row.Field<string>("Image"),
                                Status = row.Field<EntityStatus>("Status"),
                                CreatedTime = row.Field<DateTimeOffset>("CreatedTime"),
                                CreatedBy = row.Field<Guid?>("CreatedBy") != null ? row.Field<Guid>("CreatedBy") : Guid.Empty,
                                ModifiedTime = row.Field<DateTimeOffset>("ModifiedTime"),
                                ModifiedBy = row.Field<Guid?>("ModifiedBy") != null ? row.Field<Guid>("ModifiedBy") : Guid.Empty,
                                Deleted = row.Field<bool>("Deleted"),
                                DeletedBy = row.Field<Guid?>("DeletedBy") != null ? row.Field<Guid>("DeletedBy") : Guid.Empty,
                                DeletedTime = row.Field<DateTimeOffset>("DeletedTime")
                            }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Images;
        }

        public async Task<ResponseData<Images>> GetImages(ImagesGetRequest Search)
        {
            var model = new ResponseData<Images>();
            try
            {
                DataTable table = await _imagesRepo.GetImages(Search);
                model.data = (from row in table.AsEnumerable()
                              select new Images
                              {
                                  Id = row.Field<Guid>("Id"),
                                  ObjId = row.IsNull("ObjId") ? (Guid?)null : row.Field<Guid>("ObjId"),
                                  Image = row.Field<string>("Image"),
                                  Status = row.Field<EntityStatus>("Status"),
                                  CreatedTime = row.Field<DateTimeOffset>("CreatedTime"),
                                  CreatedBy = row.Field<Guid?>("CreatedBy") != null ? row.Field<Guid>("CreatedBy") : Guid.Empty,
                                  ModifiedTime = row.Field<DateTimeOffset>("ModifiedTime"),
                                  ModifiedBy = row.Field<Guid?>("ModifiedBy") != null ? row.Field<Guid>("ModifiedBy") : Guid.Empty,
                                  Deleted = row.Field<bool>("Deleted"),
                                  DeletedBy = row.Field<Guid?>("DeletedBy") != null ? row.Field<Guid>("DeletedBy") : Guid.Empty,
                                  DeletedTime = row.Field<DateTimeOffset>("DeletedTime")
                              }).ToList();
                model.CurrentPage = Search.PageIndex;
                model.PageSize = Search.PageSize;
                try
                {
                    // Thử chuyển đổi và gán giá trị
                    model.totalRecord = Convert.ToInt32(table.Rows[0]["TotalRows"]);
                }
                catch (Exception ex)
                {
                    // Nếu có lỗi xảy ra (ví dụ: không tìm thấy cột, không thể chuyển đổi), gán giá trị mặc định là 0
                    model.totalRecord = 0;
                }
                model.totalPage = (int)Math.Ceiling((double)model.totalRecord / Search.PageSize);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }

        public Task<int> UpdateImages(ImagesUpdateRequest request)
        {
            try
            {
                return _imagesRepo.UpdateImages(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
