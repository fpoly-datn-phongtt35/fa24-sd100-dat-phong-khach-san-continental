using Domain.DTO.Paging;
using Domain.DTO.Post;
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
    public class PostService : IPostService
    {
        private readonly PostRepo _postRepo;
        private readonly IConfiguration _configuration;
        public PostService(IConfiguration configuration)
        {
            _configuration = configuration;
            _postRepo = new PostRepo(configuration);
        }
        public async Task<int> AddPost(PostCreateRequest request)
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

        public async Task<ResponseData<Post>> GetAllPost(PostGetRequest Post)
        {
            var model = new ResponseData<Post>();
            try
            {
                DataTable dataTable = await _postRepo.GetAllPost(Post);
                model.data = (from row in dataTable.AsEnumerable()
                              select new Post
                              {
                                  Id = row.Field<Guid>("Id"),
                                  Title = row.Field<string>("Title"),
                                  Content = row.Field<string>("Content"),
                                  StaffId = row.Field<Guid>("StaffId"),
                                  PostTypeId = row.Field<Guid>("PostTypeId"),
                                  Status = row.Field<EntityStatus>("Status"),
                                  CreatedTime = row.Field<DateTimeOffset>("CreatedTime"),
                                  CreatedBy = row.Field<Guid?>("CreatedBy") != null ? row.Field<Guid>("CreatedBy") : Guid.Empty,
                                  ModifiedTime = row.Field<DateTimeOffset>("ModifiedTime"),
                                  ModifiedBy = row.Field<Guid?>("ModifiedBy") != null ? row.Field<Guid>("ModifiedBy") : Guid.Empty,
                                  Deleted = row.Field<bool>("Deleted"),
                                  DeletedBy = row.Field<Guid?>("DeletedBy") != null ? row.Field<Guid>("DeletedBy") : Guid.Empty,
                                  DeletedTime = row.Field<DateTimeOffset>("DeletedTime")
                              }).ToList();
                model.CurrentPage = Post.PageIndex;
                model.PageSize = Post.PageSize;
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
                model.totalPage = (int)Math.Ceiling((double)model.totalRecord / Post.PageSize);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }

        public async Task<Post> GetPostById(Guid Id)
        {
            Post Post = new Post();
            try
            {
                DataTable table = await _postRepo.GetPostById(Id);
                Post = (from row in table.AsEnumerable()
                            select new Post
                            {
                                Id = row.Field<Guid>("Id"),
                                Title = row.Field<string>("Title"),
                                Content = row.Field<string>("Content"),
                                StaffId = row.Field<Guid>("StaffId"),
                                PostTypeId = row.Field<Guid>("PostTypeId"),
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
            return Post;
        }

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
    }
}
