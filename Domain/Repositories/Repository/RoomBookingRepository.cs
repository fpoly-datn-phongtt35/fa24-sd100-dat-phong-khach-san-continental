
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
                new("@Status", roomBookingGetRequest.Status)
            };

            var dataTable = await _worker.GetDataTableAsync
                (StoredProcedureConstant.SP_GetFilteredRoomBookings, parameters);
            var roomBookings = new List<RoomBookingResponse>();

            foreach (DataRow row in dataTable.Rows)
            {
                var roomBooking = ConvertToRoomBookingRow(row);
                var roomBookingResponse = roomBooking.ToRoomBookingResponse();
                roomBookings.Add(roomBookingResponse);
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
            };
            await _worker.GetDataTableAsync(StoredProcedureConstant.SP_UpdateRoomBooking, parameters);

            return await existingRoomBooking;
        }
        catch (Exception e)
        {
            throw new Exception("An error occurred while updating the room booking", e);
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
            StaffId = Guid.Parse(row["StaffId"].ToString()!),
            Status = (RoomBookingStatus)Enum.Parse(typeof(RoomBookingStatus), row["Status"].ToString()!),
            TotalExtraPrice = row["TotalExtraPrice"] != DBNull.Value ? (decimal)row["TotalExtraPrice"] : null,
            TotalPrice = row["TotalPrice"] != DBNull.Value ? (decimal)row["TotalPrice"] : null,
            TotalRoomPrice = row["TotalRoomPrice"] != DBNull.Value ? (decimal)row["TotalRoomPrice"] : null,
            TotalServicePrice = row["TotalServicePrice"] != DBNull.Value ? (decimal)row["TotalServicePrice"] : null,
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
            Staff = new Staff()
            {
                Id = Guid.Parse(row["StaffId"].ToString()!),
                FirstName = staffFirstName,
                LastName = staffLastName
            }
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
                new SqlParameter("@NewId", SqlDbType.UniqueIdentifier) { Direction = ParameterDirection.Output }
            };

            await _worker.ExecuteNonQueryAsync(StoredProcedureConstant.SP_InsertRoomBookingForCustomer, sqlParameters);

            // Trả về ID được tạo ra
            return (Guid)sqlParameters[9].Value;
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
                    new SqlParameter("@CreatedBy",request.CreatedBy)
            };

            return _worker.ExecuteNonQuery(StoredProcedureConstant.SP_InsertRoomBooking, sqlParameters);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
