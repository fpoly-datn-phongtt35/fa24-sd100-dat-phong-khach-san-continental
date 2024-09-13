using Domain.DTO.ServiceType;
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
    public class ServiceTypeRepo : IServiceTypeRepo
    {
        private static DbWorker _DbWorker;
        private readonly IConfiguration _configuration;
        public ServiceTypeRepo(IConfiguration configuration)
        {
            _configuration = configuration;
            _DbWorker = new DbWorker(StoredProcedureConstant.Continetal);
        }
        public async Task<int> AddServiceType(ServiceTypeCreateRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Name",!string.IsNullOrEmpty(request.Name) ? request.Name : DBNull.Value),
                    new SqlParameter("@Description",!string.IsNullOrEmpty(request.Description) ? request.Description : DBNull.Value),
                    new SqlParameter("@Status",1),
                    new SqlParameter("@CreatedTime",DateTime.Now),
                    new SqlParameter("@CreatedBy", request.CreatedBy!= null ? request.CreatedBy : DBNull.Value)
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_InsertServiceType, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeleteServiceType(ServiceTypeDeleteRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", request.Id != null ? request.Id : DBNull.Value),
                    new SqlParameter("@DeletedTime", DateTime.Now),
                    new SqlParameter("@DeletedBy", request.DeletedBy != null ? request.DeletedBy : DBNull.Value)
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_DeleteServiceType, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> GetServiceTypes(ServiceTypeGetRequest Search)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Name", !string.IsNullOrEmpty(Search.Name) ? Search.Name : DBNull.Value),
                    new SqlParameter("@PageSize", Search.PageSize),
                    new SqlParameter("@PageIndex", Search.PageIndex)
                };

                return _DbWorker.GetDataTable(StoredProcedureConstant.SP_GetListServiceType, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> GetServiceTypeById(Guid Search)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", Search != null ? Search : DBNull.Value ),
                };

                return _DbWorker.GetDataTable(StoredProcedureConstant.SP_GetListServiceType, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateServiceType(ServiceTypeUpdateRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", request.Id != null ? request.Id : DBNull.Value),
                    new SqlParameter("@Name",!string.IsNullOrEmpty(request.Name) ? request.Name : DBNull.Value),
                    new SqlParameter("@Description",!string.IsNullOrEmpty(request.Description) ? request.Description : DBNull.Value),
                    new SqlParameter("@Status",1),
                    new SqlParameter("@ModifiedTime",DateTime.Now),
                    new SqlParameter("@ModifiedBy", request.ModifiedBy!= null ? request.ModifiedBy : DBNull.Value)
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_UpdateServiceType, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
