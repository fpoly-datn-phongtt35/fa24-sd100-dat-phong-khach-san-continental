
using System.Data;
using Azure.Core;
using Domain.DTO.Paging;
using Domain.DTO.RoomBooking;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Asn1.Ocsp;
using Utilities;
using Utilities.StoredProcedure;

namespace Domain.Repositories.Repository;

public class RoomBookingRepository : IRoomBookingRepository
{
    private readonly DbWorker _worker;
    private readonly IConfiguration _configuration;

    public RoomBookingRepository(IConfiguration configuration)
    {
        _worker = new DbWorker(StoredProcedureConstant.Continetal);
        _configuration = configuration;
    }

    public async Task<ResponseData<RoomBookingResponse>> GetFilteredRoomBookings
        (RoomBookingGetRequest roomBookingGetRequest)
    {
        var model = new ResponseData<RoomBookingResponse>();
        try
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new("@PageIndex", roomBookingGetRequest.PageIndex),
                new("@PageSize", roomBookingGetRequest.PageSize),
                new("@SearchString", roomBookingGetRequest.SearchString),
                new("@StaffId", roomBookingGetRequest.StaffId),
                new("@BookingType", roomBookingGetRequest.BookingType),
                new("@Status", roomBookingGetRequest.Status),
                new("@CustomerId", roomBookingGetRequest.CustomerId),
                new("@BookingBy", roomBookingGetRequest.BookingBy)

            };

            var dataTable = await _worker.GetDataTableAsync
                (StoredProcedureConstant.SP_GetFilteredRoomBookings, parameters);
            var roomBookings = new List<RoomBookingResponse>();

            foreach (DataRow row in dataTable.Rows)
            {
                try
                {
                    var roomBooking = ConvertToRoomBookingRow(row);
                    var roomBookingResponse = roomBooking.ToRoomBookingResponse();
                    roomBookings.Add(roomBookingResponse);
                }
                catch (Exception ex)
                {
                    // Log lỗi nếu một dòng bị lỗi và tiếp tục xử lý các dòng còn lại
                    Console.WriteLine($"Error converting row: {ex.Message}");
                    continue;
                }
            }

            model.data = roomBookings;
            model.CurrentPage = roomBookingGetRequest.PageIndex;
            model.PageSize = roomBookingGetRequest.PageSize;

            try
            {
                model.totalRecord = Convert.ToInt32(dataTable.Rows[0]["TotalRows"]);
            }
            catch
            {
                model.totalRecord = 0;
            }

            model.totalPage = (int)Math.Ceiling((double)model.totalRecord / roomBookingGetRequest.PageSize);
        }
        catch (Exception e)
        {
            throw new ArgumentNullException("An error occurred while getting all room bookings", e);
        }

        return model;
    }

    public async Task<RoomBooking?> GetRoomBookingById(Guid roomBookingId)
    {
        try
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = roomBookingId }
            };

            var dataTable = await _worker.GetDataTableAsync
                (StoredProcedureConstant.SP_GetRoomBookingById, parameters);

            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];
            var roomBooking = ConvertToRoomBookingRow(row);

            return roomBooking;
        }
        catch (Exception e)
        {
            throw new ArgumentNullException("An error occurred while retrieving the room booking", e);
        }
    }

    public async Task<RoomBooking?> UpdateRoomBooking(RoomBooking roomBooking)
    {
        try
        {
            var existingRoomBooking = GetRoomBookingById(roomBooking.Id);
            if (existingRoomBooking == null)
                throw new Exception("Room booking not found");

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", roomBooking.Id) ,
                new SqlParameter("@Status", roomBooking.Status) ,
                new SqlParameter("@ModifiedTime",roomBooking.ModifiedTime) ,
                new SqlParameter("@ModifiedBy", roomBooking.ModifiedBy),
                new SqlParameter("@TotalPrice", roomBooking.TotalPrice.HasValue ? roomBooking.TotalPrice : DBNull.Value),
                new SqlParameter("@TotalExtraPrice", roomBooking.TotalExtraPrice.HasValue ? roomBooking.TotalExtraPrice : DBNull.Value),
                new SqlParameter("@TotalServicePrice", roomBooking.TotalServicePrice.HasValue ? roomBooking.TotalServicePrice : DBNull.Value),
                new SqlParameter("@TotalExpenses", roomBooking.TotalExpenses.HasValue ? roomBooking.TotalExpenses : DBNull.Value),
                new SqlParameter("@TotalPriceReality", roomBooking.TotalPriceReality.HasValue ? roomBooking.TotalPriceReality : DBNull.Value),
            };
            await _worker.GetDataTableAsync(StoredProcedureConstant.SP_UpdateRoomBooking, parameters);

            return await existingRoomBooking;
        }
        catch (Exception e)
        {
            throw new Exception("An error occurred while updating the room booking", e);
        }
    }
    
    public async Task<List<DateTimeOffset>> GetCheckinRoomBookingByRoomBookingId(Guid roomBookingId)
    {
        List<DateTimeOffset> checkinTimes = new List<DateTimeOffset>();
        try
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
            {
                new SqlParameter("@RoomBookingId", roomBookingId)
            };

            // Thực thi stored procedure và lấy dữ liệu với ExecuteReaderAsync trong phạm vi using
            using (var reader = await _worker.ExecuteReaderAsync(StoredProcedureConstant
                       .SP_GetCheckinRoomBookingByRoomBookingId, sqlParameters))
            {
                // Đảm bảo reader còn mở khi gọi ReadAsync
                while (await reader.ReadAsync())
                {
                    if (reader["CheckInBooking"] != DBNull.Value)
                    {
                        checkinTimes.Add((DateTimeOffset)reader["CheckInBooking"]);
                    }
                }
            }

            return checkinTimes;
        }
        catch (Exception e)
        {
            // In lỗi để debug
            Console.WriteLine(e);
            throw;
        }
    }


    private RoomBooking ConvertToRoomBookingRow(DataRow row)
    {
        // Tach ten customer
        var customerNameParts = row["CustomerName"].ToString().Split(' ');
        var customerLastName = string.Join(" ", customerNameParts.Take(customerNameParts.Length - 1));
        var customerFirstName = customerNameParts.Last();

        var staffNameParts = row["StaffName"].ToString().Split(' ');
        var staffLastName = string.Join(" ", staffNameParts.Take(staffNameParts.Length - 1));
        var staffFirstName = staffNameParts.Last();
        return new RoomBooking()
        {
            Id = Guid.Parse(row["Id"].ToString()!),
            CustomerId = Guid.Parse(row["CustomerId"].ToString()!),
            StaffId = row["StaffId"] != DBNull.Value && Guid.TryParse(row["StaffId"].ToString(), out var staffGuid) 
                ? staffGuid 
                : (Guid?)null,
            Status = (RoomBookingStatus)Enum.Parse(typeof(RoomBookingStatus), row["Status"].ToString()!),
            TotalExtraPrice = row["TotalExtraPrice"] != DBNull.Value ? (decimal)row["TotalExtraPrice"] : null,
            TotalPrice = row["TotalPrice"] != DBNull.Value ? (decimal)row["TotalPrice"] : null,
            TotalRoomPrice = row["TotalRoomPrice"] != DBNull.Value ? (decimal)row["TotalRoomPrice"] : null,
            TotalServicePrice = row["TotalServicePrice"] != DBNull.Value ? (decimal)row["TotalServicePrice"] : null,
            TotalExpenses = row["TotalExpenses"] != DBNull.Value ? (decimal)row["TotalExpenses"] : null,
            TotalPriceReality = row["TotalPriceReality"] != DBNull.Value ? (decimal)row["TotalPriceReality"] : null,
            BookingBy = row["BookingBy"] != DBNull.Value ? (BookingBy)Enum.Parse(typeof(BookingBy), row["BookingBy"].ToString()!) : null,
            BookingType = (BookingType)Enum.Parse(typeof(BookingType), row["BookingType"].ToString()!),
            CreatedTime = ConvertDateTimeOffsetToString(row, "CreatedTime"),
            CreatedBy = ConvertGuidToString(row, "CreatedBy"),
            ModifiedTime = ConvertDateTimeOffsetToString(row, "ModifiedTime"),
            ModifiedBy = ConvertGuidToString(row, "ModifiedBy"),
            Deleted = row["Deleted"] != DBNull.Value && (bool)row["Deleted"],
            DeletedTime = ConvertDateTimeOffsetToString(row, "DeletedTime"),
            DeletedBy = ConvertGuidToString(row, "DeletedBy"),
            Customer = new Customer()
            {
                Id = Guid.Parse(row["CustomerId"].ToString()!),
                FirstName = customerFirstName,
                LastName = customerLastName
            },
            Staff = row["StaffId"] != DBNull.Value
                ? new Staff
                {
                    Id = Guid.Parse(row["StaffId"].ToString()!),
                    FirstName = staffFirstName,
                    LastName = staffLastName
                }
                : null
        };
    }

    private static DateTimeOffset ConvertDateTimeOffsetToString(DataRow row, string columnName)
    {
        if (row[columnName] != DBNull.Value)
        {
            return DateTimeOffset.Parse(row[columnName].ToString()!);
        }

        return DateTimeOffset.MinValue;
    }

    private static Guid? ConvertGuidToString(DataRow row, string columnName)
    {
        if (row[columnName] != DBNull.Value)
        {
            return Guid.Parse(row[columnName].ToString()!);
        }

        return null;
    }

    public async Task<Guid> CreateRoomBookingForCustomer(RoomBookingCreateRequestForCustomer request)
    {
        try
        {
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@BookingType", SqlDbType.Int) { Value = request.BookingType },
                new SqlParameter("@CustomerId", request.CustomerId),
                new SqlParameter("@StaffId", request.StaffId.HasValue ? (object)request.StaffId.Value : DBNull.Value),
                new SqlParameter("@Status", SqlDbType.Int) { Value = request.Status },
                new SqlParameter("@CreatedBy", request.CreatedBy),
                new SqlParameter("@TotalPrice", request.TotalPrice),
                new SqlParameter("@TotalRoomPrice", request.TotalRoomPrice.HasValue ? (object)request.TotalRoomPrice.Value : DBNull.Value),
                new SqlParameter("@TotalExtraPrice", request.TotalExtraPrice.HasValue ? (object)request.TotalExtraPrice.Value : DBNull.Value),
                new SqlParameter("@TotalServicePrice", request.TotalServicePrice.HasValue ? (object)request.TotalServicePrice.Value : DBNull.Value),
                new SqlParameter("@TotalExpense", request.TotalExpenses.HasValue ? (object)request.TotalExpenses.Value : DBNull.Value),    
                new SqlParameter("@TotalPriceReality", request.TotalPriceReality.HasValue ? (object)request.TotalPriceReality.Value : DBNull.Value),
                new SqlParameter("@BookingBy", SqlDbType.Int) { Value = (int)request.BookingBy },
                new SqlParameter("@NewId", SqlDbType.UniqueIdentifier) { Direction = ParameterDirection.Output }
            };

            await _worker.ExecuteNonQueryAsync(StoredProcedureConstant.SP_InsertRoomBookingForCustomer, sqlParameters);

            // Trả về ID được tạo ra
            return (Guid)sqlParameters[12].Value;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<int> CreateRoomBooking(RoomBookingCreateRequest request)
    {
        try
        {
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                    new SqlParameter("@BookingType", SqlDbType.Int) { Value = request.BookingType },
                    new SqlParameter("@CustomerId", request.CustomerId),
                    new SqlParameter("@StaffId", request.StaffId),
                    new SqlParameter("@Status", SqlDbType.Int) { Value = request.Status },
                    new SqlParameter("@TotalPrice", request.TotalPrice),
                    new SqlParameter("@TotalRoomPrice", request.TotalRoomPrice),
                    new SqlParameter("@TotalServicePrice", request.TotalServicePrice),
                    new SqlParameter("@TotalExtraPrice", request.TotalExtraPrice.HasValue ? (object)request.TotalExtraPrice.Value : DBNull.Value),
                    new SqlParameter("@TotalExpenses", request.TotalExpenses.HasValue ? (object)request.TotalExpenses.Value : DBNull.Value),
                    new SqlParameter("@TotalPriceReality", request.TotalPriceReality.HasValue ? (object)request.TotalPriceReality.Value : DBNull.Value),
                    new SqlParameter("@BookingBy", SqlDbType.Int) { Value = request.BookingBy },
                    new SqlParameter("@CreatedBy",request.CreatedBy)
            };

            return _worker.ExecuteNonQuery(StoredProcedureConstant.SP_InsertRoomBooking, sqlParameters);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<int> UpdateRoomBookingStatus(Guid id, int status)
    {
        try
        {
            var existingRoomBooking = GetRoomBookingById(id);
            if (existingRoomBooking == null)
                throw new Exception("Room booking not found");

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id) ,
                new SqlParameter("@Status", status) ,
            };
            return _worker.ExecuteNonQuery(StoredProcedureConstant.SP_UpdateRoomBookingStatus, parameters);
        }
        catch (Exception e)
        {
            throw new Exception("An error occurred while updating the room booking", e);
        }
    }

    public async Task<int> CheckDepositRoomBooking()
    {
        try
        {
            SqlParameter[] sqlParams = new SqlParameter[] {};
            return _worker.ExecuteNonQuery(StoredProcedureConstant.SP_CheckDepositedRoomBooking, sqlParams);
        }
        catch (Exception e)
        {
            throw new Exception("An error occurred while checking the roombooking status", e);
        }
    }
}
