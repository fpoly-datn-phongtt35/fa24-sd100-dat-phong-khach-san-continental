using Domain.DTO.PaymentHistory;
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
    public class PaymentHistoryRepository : IPaymentHistoryRepository
    {
        private static DbWorker _DbWorker;
        private readonly IConfiguration _configuration;
        public PaymentHistoryRepository(IConfiguration configuration)
        {
            _DbWorker = new DbWorker(StoredProcedureConstant.Continetal);
            _configuration = configuration;
        }
        public async Task<int> AddPaymentHistory(PaymentHistoryCreateRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@RoomBookingId", request.RoomBookingId),
                    new SqlParameter("@PaymentMethod", request.PaymentMethod),
                    new SqlParameter("@Amount", request.Amount),
                    new SqlParameter("@PaymentTime", request.PaymentTime),
                    new SqlParameter("@Note", request.Note)
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_InsertPaymentHistory, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> GetListPaymentHistory(PaymentHistoryGetRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                new SqlParameter("@RoomBookingId", request.RoomBookingId != null ? request.RoomBookingId : (object)DBNull.Value),
                new SqlParameter("@Note", request.Note),
                new SqlParameter("@PageSize", request.PageSize),
                new SqlParameter("@PageIndex", request.PageIndex)
                };

                return _DbWorker.GetDataTable(StoredProcedureConstant.SP_GetListPaymentHistory, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public async Task<DataTable> GetPaymentHistoryById(Guid id)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", id != null ? id : DBNull.Value ),
                };

                return _DbWorker.GetDataTable(StoredProcedureConstant.SP_GetPaymentHistoryById, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
