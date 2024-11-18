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
                    new SqlParameter("@CheckInBooking", request.CheckInBooking),
                    new SqlParameter("@CheckOutBooking", request.CheckOutBooking),
                    new SqlParameter("@CheckInReality", request.CheckInReality),
                    new SqlParameter("@CheckOutReality", request.CheckOutReality),
                    new SqlParameter("@Status", request.Status != null ? request.Status : DBNull.Value),
                    new SqlParameter("@ModifiedTime",DateTimeOffset.Now != null ? DateTimeOffset.Now : DBNull.Value),
                    new SqlParameter("@ModifiedBy", request.ModifiedBy!= null ? request.ModifiedBy : DBNull.Value),
                    new SqlParameter("@Deleted",request.Deleted != null ? request : false),
                    new SqlParameter("@DeletedTime",DateTime.Now),
                    new SqlParameter("@DeletedBy", request.DeletedBy!= null ? request.DeletedBy : DBNull.Value)
                };

                return _worker.ExecuteNonQuery(StoredProcedureConstant.SP_UpdateRoomBookingDetail, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
