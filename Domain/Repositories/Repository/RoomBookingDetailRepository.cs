using Domain.DTO.RoomBookingDetail;
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
    }
}
