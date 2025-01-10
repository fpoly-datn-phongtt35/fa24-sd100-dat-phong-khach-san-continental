using Domain.DTO.PostType;
using Domain.DTO.Paging;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories.Repository;
using Domain.Services.IServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Services
{
    public class PostTypeService : IPostTypeService
    {
        private readonly PostTypeRepo _postTypeRepo;
        private readonly IConfiguration _configuration;
        public PostTypeService(IConfiguration configuration)
        {
            _configuration = configuration;
            _postTypeRepo = new PostTypeRepo(configuration);
        }
        public async Task<int> AddPostType(PostTypeCreateRequest request)
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

        public async Task<ResponseData<PostType>> GetAllPostType(PostTypeGetRequest postType)
        {
            var model = new ResponseData<PostType>();
            try
            {
                DataTable dataTable = await _postTypeRepo.GetAllPostType(postType);
                model.data = (from row in dataTable.AsEnumerable()
                              select new PostType
                              {
                                  Id = row.Field<Guid>("Id"),
                                  TitleOfType = row.Field<PostTypeEnum>("TitleOfType"),
                                  Content = row.Field<string>("Content"),
                                  Status = row.Field<EntityStatus>("Status"),
                                  CreatedTime = row.Field<DateTimeOffset>("CreatedTime"),
                                  CreatedBy = row.Field<Guid?>("CreatedBy") != null ? row.Field<Guid>("CreatedBy") : Guid.Empty,
                                  ModifiedTime = row.Field<DateTimeOffset>("ModifiedTime"),
                                  ModifiedBy = row.Field<Guid?>("ModifiedBy") != null ? row.Field<Guid>("ModifiedBy") : Guid.Empty,
                                  Deleted = row.Field<bool>("Deleted"),
                                  DeletedBy = row.Field<Guid?>("DeletedBy") != null ? row.Field<Guid>("DeletedBy") : Guid.Empty,
                                  DeletedTime = row.Field<DateTimeOffset>("DeletedTime")
                              }).ToList();
                model.CurrentPage = postType.PageIndex;
                model.PageSize = postType.PageSize;
                try
                {
                    // Thử chuyển đổi và gán giá trị
                    model.totalRecord = Convert.ToInt32(dataTable.Rows[0]["TotalRows"]);
                }
                catch (Exception ex)
                {
                    // Nếu có lỗi xảy ra (ví dụ: không tìm thấy cột, không thể chuyển đổi), gán giá trị mặc định là 0
                    model.totalRecord = 0;
                }
                model.totalPage = (int)Math.Ceiling((double)model.totalRecord / postType.PageSize);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }

        public async Task<PostType> GetPostTypeById(Guid Id)
        {
            PostType PostType = new PostType();
            try
            {
                DataTable table = await _postTypeRepo.GetPostTypeById(Id);
                PostType = (from row in table.AsEnumerable()
                            select new PostType
                            {
                                Id = row.Field<Guid>("Id"),
                                TitleOfType = row.Field<PostTypeEnum>("TitleOfType"),
                                Content = row.Field<string>("Content"),
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
            return PostType;
        }

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
    }
}
