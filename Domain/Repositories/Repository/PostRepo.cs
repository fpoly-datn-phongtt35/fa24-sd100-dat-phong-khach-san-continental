using Domain.DTO.Post;
using Domain.Enums;
using Domain.Helper;
using Domain.Repositories.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.StoredProcedure;

namespace Domain.Repositories.Repository
{
    public class PostRepo : IPostRepo
    {
        private static DbWorker _DbWorker;
        private readonly IConfiguration _configuration;
        public PostRepo(IConfiguration configuration)
        {
            _DbWorker = new DbWorker(StoredProcedureConstant.Continetal);
            _configuration = configuration;
        }

        public async Task<int> AddPost(PostCreateRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Title", request.Title),
                    new SqlParameter("@Content", request.Content),
                    new SqlParameter("@StaffId", (object)request.StaffId),
                    new SqlParameter("@PostTypeId", (object)request.PostTypeId),
                    new SqlParameter("@Status", SqlDbType.Int) { Value = request.Status },
                    new SqlParameter("@CreatedTime", request.CreatedTime),
                    new SqlParameter("@CreatedBy", request.CreatedBy != null ? request.CreatedBy : DBNull.Value)
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_InsertPost, sqlParameters);
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
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", request.Id != null ? (object)request.Id : DBNull.Value),
                    new SqlParameter("@DeletedTime", DateTime.Now),
                    new SqlParameter("@DeletedBy", request.DeletedBy != Guid.Empty ? (object)request.DeletedBy : DBNull.Value)
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_DeletePost, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> GetPostById(Guid id)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", id != null ? id : DBNull.Value ),
                };

                return _DbWorker.GetDataTable(StoredProcedureConstant.SP_GetPostById, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdatePost(PostUpdateRequest request)
        {
            var existingPost = GetPostById(request.Id);
            if (existingPost == null)
            {
                throw new Exception("Post could not be found");
            }
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", request.Id),
                    new SqlParameter("@Title", request.Title),
                    new SqlParameter("@Content", request.Content),
                    new SqlParameter("@StaffId",(object) request.StaffId),
                    new SqlParameter("@PostTypeId",(object) request.PostTypeId),
                    new SqlParameter("@Status", SqlDbType.Int) { Value = request.Status },
                    new SqlParameter("@ModifiedTime",DateTime.Now),
                    new SqlParameter("@ModifiedBy", request.ModifiedBy!= null ? request.ModifiedBy : DBNull.Value)
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_UpdatePost, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<DataTable> GetAllPost(PostGetRequest Post)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                new SqlParameter("@Title", Post.Title),
                new SqlParameter("@Content", Post.Content),
                new SqlParameter("@PageSize", Post.PageSize),
                new SqlParameter("@PageIndex", Post.PageIndex)
                };

                return _DbWorker.GetDataTable(StoredProcedureConstant.SP_GetAllPost, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<PostTermsDto>> GetAllPostTerms()
        {
            try
            {
                var dataTable = await _DbWorker.GetDataTableAsync(
                    StoredProcedureConstant.SP_GetAllTerms,
                    Array.Empty<SqlParameter>()
                );

                var groupedData = dataTable.AsEnumerable()
                    .GroupBy(row => row.Field<PostTypeEnum>("PostTypeTitle"))
                    .Select(group => new PostTermsDto
                    {
                        PostTypeTitle = PostTypeHelper.DisplayPostType(group.Key),
                        PostIds = group.Select(row => row.Field<Guid>("PostId")).ToList(),
                        PostTitles = group.Select(row => row.Field<string>("PostTitle")).ToList(),
                        PostContents = group.Select(row => row.Field<string>("PostContent")).ToList()

                    }).ToList();
                return groupedData;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
