using Domain.DTO.ResidenceRegistration;
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
    public class ResidenceRegistrationRepo : IResidenceRegistrationRepo
    {
        private static DbWorker _dbWorker;
        private readonly IConfiguration _configuration;
        public ResidenceRegistrationRepo(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbWorker = new DbWorker(StoredProcedureConstant.Continetal);
        }

        public async Task<int> AddResidence(ResidenceAddRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@RoomBookingDetailId", request.RoomBookingDetailId),
                    new SqlParameter("@FullName", request.FullName),
                    new SqlParameter("@DateOfBirth", request.DateOfBirth),
                    new SqlParameter("@Gender", request.Gender),
                    new SqlParameter("@IdentityNumber", string.IsNullOrEmpty(request.IdentityNumber) ? (object)DBNull.Value : request.IdentityNumber),
                    new SqlParameter("@PhoneNumber", request.PhoneNumber),
                    //new SqlParameter("@CreatedTime", DateTimeOffset.Now),
                    //new SqlParameter("@CreatedBy", request.CreatedBy)
                };
                return _dbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_InsertResidenceRegistration, sqlParameters);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> CheckOut1Residence(Guid id)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter(@"Id", id)
                };

                return _dbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_CheckOut1Residence, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CheckOutByRBD(Guid roomBookingDetailId, DateTimeOffset checkOutTime)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter(@"RoomBookingDetailId", roomBookingDetailId),
                    new SqlParameter(@"CheckOutTime", checkOutTime)
                };
                return _dbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_CheckOutResidencesByRoomBookingDetail, sqlParameters);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> DeleteResidence(Guid id)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", id),
                };
                return _dbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_DeleteResidence, sqlParameters);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<int> GetMaximumOccupancyByRoomBookingDetailId(Guid roomBookingDetailId)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@RoomBookingDetailId", roomBookingDetailId)
                };

                var dataTable = await _dbWorker.GetDataTableAsync(StoredProcedureConstant.SP_GetMaximumOccupancyByRoomBookingDetailId, sqlParameters);

                if (dataTable.Rows.Count > 0)
                {
                    return Convert.ToInt32(dataTable.Rows[0][0]);
                }

                return -1; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> GetResidenceById(Guid id)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", id != null ? id : DBNull.Value ),
                };

                return _dbWorker.GetDataTable(StoredProcedureConstant.SP_GetListResidence, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> GetResidences(ResidenceGetRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@RoomBookingDetailId", request.RoomBookingDetailId != null ? request.RoomBookingDetailId : DBNull.Value ),
                    new SqlParameter("@FullName", request.FullName)
                };

                return _dbWorker.GetDataTable(StoredProcedureConstant.SP_GetListResidence, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> GetResidencesByDate(ResidenceGetByDateRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Date", request.Date),
                    new SqlParameter("@FullName", request.FullName),
                    new SqlParameter("@IdentityNumber", request.IdentityNumber),
                    new SqlParameter("@RoomName", request.RoomName),
                    new SqlParameter("@IsCheckOut", request.IsCheckOut)
                };

                return _dbWorker.GetDataTable(StoredProcedureConstant.SP_GetResidenceRegistrationByDate, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateResidence(ResidenceUpdateRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter(@"Id", request.Id),
                    new SqlParameter("@FullName", request.FullName),
                    new SqlParameter("@DateOfBirth", request.DateOfBirth),
                    new SqlParameter("@Gender", request.Gender),
                    new SqlParameter("@IdentityNumber", (object)request.IdentityNumber ?? DBNull.Value),
                    new SqlParameter("@PhoneNumber", (object)request.PhoneNumber ?? DBNull.Value)
                };

                return _dbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_UpdateResidence, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
