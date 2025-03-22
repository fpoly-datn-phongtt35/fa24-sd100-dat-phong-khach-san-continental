using Domain.DTO.Feedback;
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
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly DbWorker _DbWorker;
        private readonly IConfiguration _configuration;
        public FeedbackRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _DbWorker = new DbWorker(StoredProcedureConstant.Continetal);
        }
        public async Task<int> AddFeedback(FeedbackCreateRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@RoomBookingDetailId", request.RoomBookingDetailId),
                    new SqlParameter("@Comments", request.Comments),
                    new SqlParameter("@Rating", request.Rating),
                };
                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_InsertFeedback, sqlParameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> DeleteFeedback(FeedbackDeleteRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", request.Id),
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_DeleteFeedback, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> GetAllFeedbacks(FeedbackGetRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@PageSize", request.PageSize),
                    new SqlParameter("@PageIndex", request.PageIndex),
                    new SqlParameter("@Id", request.Id == Guid.Empty ? DBNull.Value : request.Id),
                    new SqlParameter("@CustomerId", request.CustomerId.HasValue ? request.CustomerId : DBNull.Value),
                    new SqlParameter("@RoomId", request.RoomId.HasValue ? request.RoomId : DBNull.Value),
                    new SqlParameter("@Rating", request.Rating.HasValue ? request.Rating : DBNull.Value),
                    new SqlParameter("@FromDate", request.FromDate.HasValue ? request.FromDate : DBNull.Value),
                    new SqlParameter("@ToDate", request.ToDate.HasValue ? request.ToDate : DBNull.Value),
                    new SqlParameter("@Status", request.Status.HasValue ? (int)request.Status : DBNull.Value)
                };

                return _DbWorker.GetDataTable(StoredProcedureConstant.SP_GetListFeedback, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> GetFeedbackById(Guid id)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", id)
                };

                return _DbWorker.GetDataTable(StoredProcedureConstant.SP_GetListFeedback, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateFeedback(FeedbackUpdateRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", request.Id),
                    new SqlParameter("@Comments", request.Comments),
                    new SqlParameter("@Rating", request.Rating),
                    new SqlParameter("@Status", request.Status),
                    new SqlParameter("@Deleted", request.Deleted),
                    new SqlParameter("@ModifiedTime", DateTimeOffset.UtcNow),
                    new SqlParameter("@ModifiedBy", request.ModifiedBy)
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_UpdateFeedback, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
