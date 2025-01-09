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
using Domain.Enums;
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


        public async Task<RoomBookingDetail?> UpdateRoomBookingDetail2(RoomBookingDetail roomBookingDetail)
        {
            var existingRoomBookingDetail = GetRoomBookingDetailById2(roomBookingDetail.Id);
            if (existingRoomBookingDetail == null)
                throw new Exception("RoomBookingDetail could not be found");

            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new("@Id", SqlDbType.UniqueIdentifier) { Value = roomBookingDetail.Id },
                    new("@CheckInBooking", (object)roomBookingDetail.CheckInBooking ?? DBNull.Value),
                    new("@CheckOutBooking", (object)roomBookingDetail.CheckOutBooking ?? DBNull.Value),
                    new("@CheckInReality", (object)roomBookingDetail.CheckInReality ?? DBNull.Value),
                    new("@CheckOutReality", (object)roomBookingDetail.CheckOutReality ?? DBNull.Value),
                    new("@ServicePrice", (object)roomBookingDetail.ServicePrice ?? DBNull.Value),
                    new("@ExtraService", (object)roomBookingDetail.ExtraService ?? DBNull.Value),
                    new("@Note", (object)roomBookingDetail.Note ?? DBNull.Value),
                    new("@Status", (object)roomBookingDetail.Status ?? DBNull.Value),
                    new("@ModifiedBy", (object)roomBookingDetail.ModifiedBy ?? DBNull.Value),
                    new("@ModifiedTime", SqlDbType.DateTimeOffset) { Value = DateTimeOffset.Now },
                    new("@Expenses", SqlDbType.Decimal) { Value = (object)roomBookingDetail.Expenses ?? DBNull.Value },
                    new("@ExtraPrice", SqlDbType.Decimal) { Value = (object)roomBookingDetail.ExtraPrice ?? DBNull.Value },
                };
                foreach (var param in sqlParameters)
                {
                    Console.WriteLine($"{param.ParameterName}: {param.Value}");
                }

                await _worker.ExecuteNonQueryAsync(StoredProcedureConstant.SP_UpdateRoomBookingDetail, sqlParameters);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                Console.WriteLine(ex);
                throw new Exception("An error occurred while updating the room booking detail", ex);
            }

            return await existingRoomBookingDetail;
        }

        public async Task<int> UpSertRoomBookingDetail(RoomBookingDetail request)
        {
            try
            {
                if (request.Id == Guid.Empty)
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
                        new SqlParameter("@Expenses", request.Expenses),
                        new SqlParameter("@Note", request.Note),
                        new SqlParameter("@Status", SqlDbType.Int) { Value = request.Status },
                        new SqlParameter("@CreatedBy", request.CreatedBy)
                    };

                    _worker.ExecuteNonQuery(StoredProcedureConstant.SP_InsertRoomBookingDetail, sqlParameters);
                }
                else
                {
                    var existingRoomBookingDetail = GetById(request.Id);
                    if (existingRoomBookingDetail == null)
                    {
                        throw new Exception("RoomBookingDetail could not be found");
                    }

                    SqlParameter[] sqlParameters = new SqlParameter[]
                    {
                        new SqlParameter("@Id", request.Id),
                        new SqlParameter("@CheckInBooking", (object)request.CheckInBooking ?? DBNull.Value),
                        new SqlParameter("@CheckOutBooking", (object)request.CheckOutBooking ?? DBNull.Value),
                        new SqlParameter("@CheckInReality", (object)request.CheckInReality ?? DBNull.Value),
                        new SqlParameter("@CheckOutReality", (object)request.CheckOutReality ?? DBNull.Value),
                        new SqlParameter("@Status", (int)request.Status), // Chuyển đổi enum sang int
                        new SqlParameter("@ExtraPrice", (object)request.ExtraPrice ?? DBNull.Value),
                        new SqlParameter("@Expenses", (object)request.Expenses ?? DBNull.Value),
                        new SqlParameter("@ServicePrice", (object)request.ServicePrice ?? DBNull.Value),
                        new SqlParameter("@ExtraService", (object)request.ExtraService ?? DBNull.Value),
                        new SqlParameter("@Note", (object)request.Note ?? DBNull.Value),
                        new SqlParameter("@ModifiedTime", (object)DateTimeOffset.Now ?? DBNull.Value),
                        new SqlParameter("@ModifiedBy", (object)request.ModifiedBy ?? DBNull.Value),
                        new SqlParameter("@Deleted", request.Deleted),
                        new SqlParameter("@DeletedTime", (object)(request.Deleted ? DateTimeOffset.Now : DBNull.Value)),
                        new SqlParameter("@DeletedBy", (object)request.DeletedBy ?? DBNull.Value)
                    };
                    _worker.ExecuteNonQuery(StoredProcedureConstant.SP_UpdateRoomBookingDetail, sqlParameters);
                }

                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                    new SqlParameter("@ExtraPrice", request.ExtraPrice ?? (object)DBNull.Value),
                    new SqlParameter("@Deposit", request.Deposit ?? (object)DBNull.Value),
                    new SqlParameter("@Expenses", request.Expenses ?? (object)DBNull.Value),
                    new SqlParameter("@Note", request.Note ?? (object)DBNull.Value),
                    new SqlParameter("@Status", SqlDbType.Int) { Value = request.Status },
                    new SqlParameter("@CreatedBy", request.CreatedBy)
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
                    new SqlParameter("@CreatedBy", request.CreatedBy)
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
                    new SqlParameter("@RoomBookingId ", id != null ? id : DBNull.Value),
                };

                return _worker.GetDataTable(StoredProcedureConstant.SP_GetListRoomBookingDetailByRoomBookingId,
                    sqlParameters);
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
                    new SqlParameter("@Id", id != null ? id : DBNull.Value),
                };

                return _worker.GetDataTable(StoredProcedureConstant.SP_GetRoomBookingDetailById, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<RoomBookingDetail?> GetRoomBookingDetailById2(Guid roomBookingDetailId)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new("@Id", SqlDbType.UniqueIdentifier) { Value = roomBookingDetailId }
                };

                var dataTable = await _worker.GetDataTableAsync(StoredProcedureConstant.SP_GetRoomBookingDetailById2,
                    sqlParameters);

                if (dataTable.Rows.Count == 0)
                    return null;

                var row = dataTable.Rows[0];
                var roomBookingDetail = ConvertDataRowToRoomBookingDetail(row);

                return roomBookingDetail;
            }
            catch (Exception e)
            {
                throw new ArgumentNullException("An error occurred while retrieving the room booking details", e);
            }
        }

        public async Task<RoomBookingDetail?> GetRoomBookingDetailWithEditHistory(Guid roomBookingDetailId)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new("@Id", SqlDbType.UniqueIdentifier) { Value = roomBookingDetailId }
                };

                var roomBookingDetailDataTable = await _worker
                    .GetDataTableAsync(StoredProcedureConstant.SP_GetRoomBookingDetailById2, parameters);
                if (roomBookingDetailDataTable.Rows.Count == 0)
                    return null;

                var row = roomBookingDetailDataTable.Rows[0];
                var roomBookingDetail = ConvertDataRowToRoomBookingDetail(row);

                //Get List EditHistory to RoomBookingDetail
                roomBookingDetail.EditHistory = await GetEditHistoryByRoomBookingDetailId(roomBookingDetailId);

                return roomBookingDetail;
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while retrieving the room booking detail with edit history", e);
            }
        }

        public async Task<List<EditHistory>> GetEditHistoryByRoomBookingDetailId(Guid roomBookingDetailId)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new("@RoomBookingDetailId", SqlDbType.UniqueIdentifier) { Value = roomBookingDetailId }
                };
                
                var dataTable = await _worker
                    .GetDataTableAsync(StoredProcedureConstant.SP_GetEditHistoryByRoomBookingDetailId, parameters);
                if(dataTable.Rows.Count == 0)
                    return new List<EditHistory>(); // Ko co data trả về
                
                var editHistories = new List<EditHistory>();
                foreach (DataRow row in dataTable.Rows)
                {
                    if (dataTable.Columns.Contains("Id"))
                    {
                        var editHistory = new EditHistory()
                        {
                            Id = (int)row["Id"],
                            RoomBookingDetailId = (Guid)row["RoomBookingDetailId"],
                            For = (For)(int)row["For"],
                            Content = row["Content"].ToString(),
                            Description = row["Description"].ToString(),
                            ModifiedAt = (DateTimeOffset)row["ModifiedAt"],
                        };
                        editHistories.Add(editHistory);
                    }
                    else
                    {
                        throw new ArgumentException("Column 'Id' does not exist in the result set.");
                    }
                }

                return editHistories;
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while retrieving the list edit history", e);
            }
        }

        public async Task<int> UpdateRoomBookingDetail(RoomBookingDetailUpdateRequest request)
        {
            var existingRoomBookingDetail = GetRoomBookingDetailById2(request.Id);
            if (existingRoomBookingDetail == null)
            {
                throw new Exception("RoomBookingDetail could not be found");
            }

            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new("@Id", SqlDbType.UniqueIdentifier) { Value = request.Id },
                    new("@CheckInBooking", SqlDbType.DateTimeOffset) { Value = request.CheckInBooking },
                    new("@CheckOutBooking", SqlDbType.DateTimeOffset) { Value = request.CheckOutBooking },
                    new("@CheckInReality", SqlDbType.DateTimeOffset) { Value = request.CheckInReality },
                    new("@CheckOutReality", SqlDbType.DateTimeOffset) { Value = request.CheckOutReality },
                    new("@ServicePrice", SqlDbType.Decimal){Value = request.ServicePrice},
                    new("@ExtraService", SqlDbType.Decimal){Value = request.ExtraService},
                    new("@Expenses", SqlDbType.Decimal) { Value = request.Expenses },
                    new("@Note", SqlDbType.NVarChar) { Value = request.Note },
                    new("@Status", SqlDbType.Int) { Value = request.Status },
                    new("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = request.ModifiedBy },
                    new("@ModifiedTime", SqlDbType.DateTimeOffset) { Value = request.ModifiedTime },
                    new("@DeletedTime", SqlDbType.DateTimeOffset) { Value = request.DeletedTime },
                    new("@Deleted", SqlDbType.Bit) { Value = request.Deleted },
                    new("@DeletedBy", SqlDbType.UniqueIdentifier) { Value = request.DeletedBy },

                    new("@TotalPrice", SqlDbType.Decimal) { Direction = ParameterDirection.Output },
                    new("@NumberOfNights", SqlDbType.Int) { Direction = ParameterDirection.Output },
                    new("@ExtraCharge", SqlDbType.Decimal) { Direction = ParameterDirection.Output },
                    new("@Expenses", SqlDbType.Decimal) { Direction = ParameterDirection.Output }
                };

                var totalPrice = sqlParameters[13].Value;
                var numberOfNights = sqlParameters[14].Value;
                var extraCharge = sqlParameters[15].Value;
                var expenses = sqlParameters[16].Value;

                Console.WriteLine(
                    $"OUTPUT Params: TotalPrice={totalPrice}, NumberOfNights={numberOfNights}, ExtraCharge={extraCharge}, Expenses={expenses}");

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
                    new SqlParameter("@CustomerId", customerId != null ? customerId : DBNull.Value),
                };

                return _worker.GetDataTable(StoredProcedureConstant.SP_GetRoomBookingDetailsByCustomerId,
                    sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public RoomBookingDetail ConvertDataRowToRoomBookingDetail(DataRow row)
        {
            return new RoomBookingDetail()
            {
                Id = row["Id"] != DBNull.Value ? Guid.Parse(row["Id"].ToString()!) : Guid.Empty,
                RoomBookingId = row["RoomBookingId"] != DBNull.Value
                    ? Guid.Parse(row["RoomBookingId"].ToString()!)
                    : Guid.Empty,
                RoomId = row["RoomId"] != DBNull.Value ? Guid.Parse(row["RoomId"].ToString()!) : Guid.Empty,
                CheckInBooking = ConvertDateTimeOffsetToString(row, "CheckInBooking"),
                CheckOutBooking = ConvertDateTimeOffsetToString(row, "CheckOutBooking"),
                CheckInReality = ConvertDateTimeOffsetToString(row, "CheckInReality"),
                CheckOutReality = ConvertDateTimeOffsetToString(row, "CheckOutReality"),
                Price = row["Price"] != DBNull.Value ? decimal.Parse(row["Price"].ToString()!) : 0,
                Expenses = row["Expenses"] != DBNull.Value ? decimal.Parse(row["Expenses"].ToString()!) : 0,
                ExtraPrice = row["ExtraPrice"] != DBNull.Value ? decimal.Parse(row["ExtraPrice"].ToString()!) : 0,
                Deposit = row["Deposit"] != DBNull.Value ? decimal.Parse(row["Deposit"].ToString()!) : 0,
                Note = row["Note"] != DBNull.Value ? row["Note"].ToString()! : string.Empty,
                Status = row["Status"] != DBNull.Value
                    ? (EntityStatus)Enum.Parse(typeof(EntityStatus), row["Status"].ToString()!)
                    : EntityStatus.Active,
                CreatedTime = ConvertDateTimeOffsetToString(row, "CreatedTime"),
                CreatedBy = ConvertGuidToString(row, "CreatedBy"),
                ModifiedTime = ConvertDateTimeOffsetToString(row, "ModifiedTime"),
                ModifiedBy = ConvertGuidToString(row, "ModifiedBy"),
                Deleted = row["Deleted"] != DBNull.Value && (bool)row["Deleted"],
                DeletedTime = ConvertDateTimeOffsetToString(row, "DeletedTime"),
                DeletedBy = ConvertGuidToString(row, "DeletedBy"),
                Room = new Room()
                {
                    Id = row["RoomId"] != DBNull.Value ? Guid.Parse(row["RoomId"].ToString()!) : Guid.Empty,
                    Name = row["RoomName"] != DBNull.Value ? row["RoomName"].ToString()! : string.Empty,
                    Price = row["RoomPrice"] != DBNull.Value ? decimal.Parse(row["RoomPrice"].ToString()!) : 0
                }
            };
        }

        private static DateTimeOffset? ConvertDateTimeOffsetToString(DataRow row, string columnName)
        {
            if (row[columnName] != DBNull.Value)
            {
                return DateTimeOffset.Parse(row[columnName].ToString()!);
            }

            return null;
        }

        private static Guid? ConvertGuidToString(DataRow row, string columnName)
        {
            if (row[columnName] != DBNull.Value)
            {
                return Guid.Parse(row[columnName].ToString()!);
            }

            return null;
        }
    }
}