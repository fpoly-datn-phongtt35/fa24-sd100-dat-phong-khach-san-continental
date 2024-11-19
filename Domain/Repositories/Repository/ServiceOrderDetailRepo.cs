using Domain.DTO.ServiceOrderDetail;
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
    public class ServiceOrderDetailRepo : IServiceOrderDetailRepo
    {
        private readonly DbWorker _DbWorker;
        private readonly IConfiguration _configuration;
        public ServiceOrderDetailRepo(IConfiguration configuration)
        {
            _configuration = configuration;
            _DbWorker = new DbWorker(StoredProcedureConstant.Continetal);
        }

        public async Task<int> AddServiceOrderDetail(ServiceOrderDetailCreateRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@RoomBookingId", request.RoomBookingId),
                    new SqlParameter("@ServiceId", request.ServiceId),
                    new SqlParameter("@Amount", request.Amount),
                    new SqlParameter("@Quantity", request.Quantity),
                    new SqlParameter("@Price", request.Price),
                    new SqlParameter("@Status", (int)request.Status),
                    new SqlParameter("@CreatedTime", request.CreatedTime),
                    new SqlParameter("@CreatedBy", request.CreatedBy != null ? request.CreatedBy : DBNull.Value)
                };
                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_InsertServiceOrderDetail, sqlParameters);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<DataTable> GetServiceOrderDetailById(Guid id)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", id != null ? id : DBNull.Value ),
                };

                return _DbWorker.GetDataTable(StoredProcedureConstant.SP_GetServiceOrderDetailById, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DataTable> GetServiceOrderDetailByRoomBookingId(Guid id)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@RoomBookingId", id != null ? id : DBNull.Value ),
                };

                return _DbWorker.GetDataTable(StoredProcedureConstant.SP_GetListServiceOrderDetail, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeleteServiceOrderDetail(ServiceOrderDetailDeleteRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", request.Id != null ? (object)request.Id : DBNull.Value),
                    new SqlParameter("@DeletedTime", DateTime.Now),
                    new SqlParameter("@DeletedBy", request.DeletedBy != Guid.Empty ? (object)request.DeletedBy : DBNull.Value)
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_DeleteServiceOrderDetail, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> GetServiceOrderDetails(ServiceOrderDetailGetRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@PageSize", request.PageSize),
                    new SqlParameter("@PageIndex", request.PageIndex)
                };

                return _DbWorker.GetDataTable(StoredProcedureConstant.SP_GetListServiceOrderDetail, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateServiceOrderDetail(ServiceOrderDetailUpdateRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", request.Id != Guid.Empty ? request.Id : (object)DBNull.Value),
                    new SqlParameter("@RoomBookingId", request.RoomBookingId),
                    new SqlParameter("@ServiceId", request.ServiceId),
                    new SqlParameter("@Amount", request.Amount),
                    new SqlParameter("@Price", request.Price),
                    new SqlParameter("@Status", request.Status),
                    new SqlParameter("@Deleted", request.Deleted),
                    new SqlParameter("@ModifiedTime", DateTimeOffset.Now),
                    new SqlParameter("@ModifiedBy", request.ModifiedBy != null ? request.ModifiedBy : (object)DBNull.Value)
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_UpdateServiceOrderDetail, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
