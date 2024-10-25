using Domain.DTO.VoucherDetail;
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
    public class VoucherDetailRepo : IVoucherDetailRepo
    {
        private readonly DbWorker _DbWorker;
        private readonly IConfiguration _configuration;

        public VoucherDetailRepo(IConfiguration configuration)
        {
            _configuration = configuration;
            _DbWorker = new DbWorker(StoredProcedureConstant.Continetal);
        }

        public async Task<int> AddVoucherDetail(VoucherDetailCreateRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter ("@RoomBookingId", request.RoomBookingId),
                    new SqlParameter("@VoucherId", request.VoucherId),
                    new SqlParameter("@Code", request.Code),
                    new SqlParameter("@StartDate", request.StartDate),
                    new SqlParameter("@EndDate", request.EndDate),
                    new SqlParameter("@Status", request.Status),
                    new SqlParameter("@CreatedTime", DateTime.Now),
                    new SqlParameter("@CreatedBy", request.CreatedBy)
                };
                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_InsertVoucherDetail, sqlParameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> DeleteVoucherDetail(VoucherDetailDeleteRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", request.Id),
                    new SqlParameter("@DeletedTime", DateTime.Now)
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_DeleteVoucherDetail, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> GetVoucherDetails(VoucherDetailGetRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@PageSize", request.PageSize),
                    new SqlParameter("@PageIndex", request.PageIndex)
                };

                return _DbWorker.GetDataTable(StoredProcedureConstant.SP_GetListVoucherDetail, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> GetVoucherDetailById(Guid id)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", id)
                };

                return _DbWorker.GetDataTable(StoredProcedureConstant.SP_GetListVoucherDetail, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateVoucherDetail(VoucherDetailUpdateRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter ("@Id", request.Id),
                    new SqlParameter ("@RoomBookingId", request.RoomBookingId),
                    new SqlParameter("@VoucherId", request.VoucherId),
                    new SqlParameter("@Code", request.Code),
                    new SqlParameter("@StartDate", request.StartDate),
                    new SqlParameter("@EndDate", request.EndDate),
                    new SqlParameter("@Status", request.Status),
                    new SqlParameter("@Deleted", request.Deleted),
                    new SqlParameter("@ModifiedTime", DateTime.Now)
                };
                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_UpdateVoucherDetail, sqlParameters);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
