using Domain.DTO.PostType;
using Domain.Models;
using Domain.Repositories.IRepository;
using Microsoft.Data.SqlClient;
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
    public class PostTypeRepo : IPostTypeRepo
    {
        private static DbWorker _DbWorker;
        private readonly IConfiguration _configuration;
        public PostTypeRepo(IConfiguration configuration)
        {
            _DbWorker = new DbWorker(StoredProcedureConstant.Continetal);
            _configuration = configuration;
        }

        public async Task<int> AddPostType(PostTypeCreateRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@TitleOfType", request.TitleOfType),
                    new SqlParameter("@Content", request.Content),
                    new SqlParameter("@Status", (int)request.Status),
                    new SqlParameter("@CreatedTime", request.CreatedTime),
                    new SqlParameter("@CreatedBy", request.CreatedBy != null ? request.CreatedBy : DBNull.Value)
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_InsertPostType, sqlParameters);
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
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", request.Id != null ? (object)request.Id : DBNull.Value),
                    new SqlParameter("@DeletedTime", DateTime.Now),
                    new SqlParameter("@DeletedBy", request.DeletedBy != Guid.Empty ? (object)request.DeletedBy : DBNull.Value)
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_DeletePostType, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> GetPostTypeById(Guid id)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", id != null ? id : DBNull.Value ),
                };

                return _DbWorker.GetDataTable(StoredProcedureConstant.SP_GetPostTypeById, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdatePostType(PostTypeUpdateRequest request)
        {
            var existingPostType = GetPostTypeById(request.Id);
            if (existingPostType == null)
            {
                throw new Exception("Post type could not be found");
            }
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", request.Id),
                    new SqlParameter("@TitleOfType", request.TitleOfType),
                    new SqlParameter("@Content", request.Content),
                    new SqlParameter("@Status", SqlDbType.Int) { Value = request.Status },
                    new SqlParameter("@ModifiedTime",DateTime.Now),
                    new SqlParameter("@ModifiedBy", request.ModifiedBy!= null ? request.ModifiedBy : DBNull.Value)
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_UpdatePostType, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<DataTable> GetAllPostType(PostTypeGetRequest PostType)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                new SqlParameter("@TitleOfType", PostType.TitleOfType),
                new SqlParameter("@PageSize", PostType.PageSize),
                new SqlParameter("@PageIndex", PostType.PageIndex)
                };

                return _DbWorker.GetDataTable(StoredProcedureConstant.SP_GetAllPostType, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
