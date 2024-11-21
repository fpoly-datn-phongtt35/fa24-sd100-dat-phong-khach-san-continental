using Domain.DTO.RoomBookingDetail;
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
    public class RoomBookingDetailRepository : IRoomBookingDetailRepository
    {
        private readonly DbWorker _worker;
        private readonly IConfiguration _configuration;

        public RoomBookingDetailRepository(IConfiguration configuration)
        {
            _worker = new DbWorker(StoredProcedureConstant.Continetal);
            _configuration = configuration;
        }

        public async Task<int> CreateRoomBookingDetail(RoomBookingDetailCreateRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@RoomId", request.RoomId),
                    new SqlParameter("@RoomBookingId", request.RoomBookingId),
                    new SqlParameter("@CheckInBooking", request.CheckInBooking),
                    new SqlParameter("@CheckOutBooking", request.CheckOutBooking),
                    new SqlParameter("@CheckInReality", request.CheckInReality),
                    new SqlParameter("@CheckOutReality", request.CheckOutReality),
                    new SqlParameter("@Price", request.Price),
                    new SqlParameter("@ExtraPrice", request.ExtraPrice),
                    new SqlParameter("@Deposit", request.Deposit),
                    new SqlParameter("@Status", SqlDbType.Int) { Value = request.Status },
                    new SqlParameter("@CreatedBy",request.CreatedBy)
                };

                return _worker.ExecuteNonQuery(StoredProcedureConstant.SP_InsertRoomBookingDetail, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CreateRoomBookingDetailForCustomer(RoomBookingDetailCreateRequestForCustomer request)
        {

            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@RoomIds", request.RoomIdsAsString),
                    new SqlParameter("@RoomBookingId", request.RoomBookingId),
                    new SqlParameter("@CheckInBooking", request.CheckInBooking),
                    new SqlParameter("@CheckOutBooking", request.CheckOutBooking),
                    new SqlParameter("@CheckInReality", request.CheckInReality),
                    new SqlParameter("@CheckOutReality", request.CheckOutReality),
                    new SqlParameter("@Price", request.Price),
                    new SqlParameter("@ExtraPrice", request.ExtraPrice),
                    new SqlParameter("@Deposit", request.Deposit),
                    new SqlParameter("@Status", SqlDbType.Int) { Value = request.Status },
                    new SqlParameter("@CreatedTime", request.CreatedTime),
                    new SqlParameter("@CreatedBy",request.CreatedBy)
                };

                return _worker.ExecuteNonQuery(StoredProcedureConstant.SP_BookRoomDetailForCustomer, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DataTable> GetListRoomBookingDetailByRoomBookingId(Guid id)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@RoomBookingId ", id != null ? id : DBNull.Value ),
                };

                return _worker.GetDataTable(StoredProcedureConstant.SP_GetListRoomBookingDetailByRoomBookingId, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DataTable> GetById(Guid id)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", id != null ? id : DBNull.Value ),
                };

                return _worker.GetDataTable(StoredProcedureConstant.SP_GetRoomBookingDetailById, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateRoomBookingDetail(RoomBookingDetailUpdateRequest request)
        {
            var existingRoomBookingDetail = GetById(request.Id);
            if (existingRoomBookingDetail == null)
            {
                throw new Exception("RoomBookingDetail could not be found");
            }
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
               {
                    new SqlParameter("@Id", request.Id),
                    new SqlParameter("@CheckInBooking", (object)request.CheckInBooking ?? DBNull.Value),
                    new SqlParameter("@CheckOutBooking", (object)request.CheckOutBooking ?? DBNull.Value),
                    new SqlParameter("@CheckInReality", (object)request.CheckInReality ?? DBNull.Value),
                    new SqlParameter("@CheckOutReality", (object)request.CheckOutReality ?? DBNull.Value),
                    new SqlParameter("@Status", (int)request.Status), // Chuyển đổi enum sang int
                    new SqlParameter("@ExtraPrice", (object)request.ExtraPrice ?? DBNull.Value) ,
                    new SqlParameter("@ModifiedTime", (object)DateTimeOffset.Now ?? DBNull.Value),
                    new SqlParameter("@ModifiedBy", (object)request.ModifiedBy ?? DBNull.Value),
                    new SqlParameter("@Deleted", request.Deleted),
                    new SqlParameter("@DeletedTime", (object)(request.Deleted ? DateTimeOffset.Now : DBNull.Value)),
                    new SqlParameter("@DeletedBy", (object)request.DeletedBy ?? DBNull.Value)
               };


                return _worker.ExecuteNonQuery(StoredProcedureConstant.SP_UpdateRoomBookingDetail, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> GetRoomBookingDetailByCustomerId(Guid customerId)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@CustomerId", customerId != null ? customerId : DBNull.Value ),
                };

                return _worker.GetDataTable(StoredProcedureConstant.SP_GetRoomBookingDetailsByCustomerId, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
