using Domain.DTO.ServiceOrder;
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
    public class ServiceOrderRepo : IServiceOrderRepo
    {
        private readonly DbWorker _DbWorker;
        private readonly IConfiguration _configuration;
        public ServiceOrderRepo(IConfiguration configuration)
        {
            _configuration = configuration;
            _DbWorker = new DbWorker(StoredProcedureConstant.Continetal);
        }
        public async Task<int> AddServiceOrder(ServiceOrderCreateRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("RoomBookingDetailId", request.RoomBookingDetailId != null ? request.RoomBookingDetailId : DBNull.Value),
                    new SqlParameter("Status",1),
                    new SqlParameter("CreatedTime", DateTime.Now),
                    new SqlParameter("CreatedBy", request.CreatedBy != null ? request.CreatedBy : DBNull.Value)
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_InsertServiceOrder, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeleteServiceOrder(ServiceOrderDeleteRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", request.Id != null ? request.Id : DBNull.Value),
                    new SqlParameter("@DeletedTime", DateTime.Now),
                    new SqlParameter("@DeletedBy", request.DeletedBy.HasValue ? (object) request.DeletedBy : DBNull.Value)
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_DeleteServiceOrder, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> GetServiceOrders(ServiceOrderGetRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@RoomBookingDetailId", request.RoomBookingDetailId.HasValue ? (object)request.RoomBookingDetailId.Value : DBNull.Value),
                    new SqlParameter("@PageSize", request.PageSize),
                    new SqlParameter("@PageIndex", request.PageIndex),
                };

                return _DbWorker.GetDataTable(StoredProcedureConstant.SP_GetListServiceOrder, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateServiceOrder(ServiceOrderUpdateRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", request.Id != null ? request.Id : DBNull.Value),
                    new SqlParameter("@RoomBookingDetailId", request.RoomBookingDetailId.HasValue ? (object)request.Id : DBNull.Value),
                    new SqlParameter("@Status",1),
                    new SqlParameter("@ModifiedTime",DateTime.Now),
                    new SqlParameter("@ModifiedBy", request.ModifiedBy.HasValue ? (object)request.ModifiedBy : DBNull.Value)
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_UpdateServiceOrder, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> GetServiceOrderById(Guid id)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", id),
                };

                return _DbWorker.GetDataTable(StoredProcedureConstant.SP_GetServiceOrderById, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
