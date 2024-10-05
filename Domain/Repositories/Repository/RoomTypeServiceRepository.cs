using System.Data;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Utilities.StoredProcedure;

namespace Domain.Repositories.Repository;

public class RoomTypeServiceRepository : IRoomTypeServiceRepository
{
    private readonly DbWorker _worker;
    private readonly IConfiguration _configuration;

    public RoomTypeServiceRepository(IConfiguration configuration)
    {
        _worker = new DbWorker(StoredProcedureConstant.Continetal);
        _configuration = configuration;
    }

    public async Task<List<RoomTypeService>> GetAllRoomTypeServices()
    {
        try
        {
            var roomTypeServices = new List<RoomTypeService>();
            var dataTable = await _worker.GetDataTableAsync(StoredProcedureConstant.SP_GetAllRoomTypeServices, null);

            foreach (DataRow row in dataTable.Rows)
            {
                var roomTypeService = ConvertDataRowToRoomTypeService(row);
                roomTypeServices.Add(roomTypeService);
            }

            return roomTypeServices;
        }
        catch (Exception e)
        {
            throw new ArgumentNullException("An error occurred while getting all room types", e);
        }
    }

    public async Task<RoomTypeService?> GetRoomTypeServiceById(Guid roomTypeServiceId)
    {
        try
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = roomTypeServiceId }
            };

            var dataTable =
                await _worker.GetDataTableAsync(StoredProcedureConstant.SP_GetRoomTypeServiceById, parameters);

            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];
            var roomTypeService = ConvertDataRowToRoomTypeService(row);

            return roomTypeService;
        }
        catch (Exception e)
        {
            throw new ArgumentNullException("An error occurred while retrieving the amenity room", e);
        }
    }

    public async Task<RoomTypeService> CreateRoomTypeService(RoomTypeService roomTypeService)
    {
        try
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ServiceId", SqlDbType.UniqueIdentifier) { Value = roomTypeService.ServiceId },
                new SqlParameter("@RoomTypeId", SqlDbType.UniqueIdentifier) { Value = roomTypeService.RoomTypeId },
                new SqlParameter("@Amount", SqlDbType.Int) { Value = roomTypeService.Amount },
                new SqlParameter("@Status", SqlDbType.Int) { Value = roomTypeService.Status },
                new SqlParameter("@CreatedTime", SqlDbType.DateTimeOffset) { Value = DateTimeOffset.Now },
                new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier) { Value = roomTypeService.CreatedBy },
                new SqlParameter("@ModifiedTime", SqlDbType.DateTimeOffset) { Value = roomTypeService.ModifiedTime },
                new SqlParameter("@Deleted", SqlDbType.Bit) { Value = roomTypeService.Deleted },
                new SqlParameter("@DeletedTime", SqlDbType.DateTimeOffset) { Value = roomTypeService.DeletedTime },
            };

            await _worker.GetDataTableAsync(StoredProcedureConstant.SP_InsertRoomTypeService, parameters);
            return roomTypeService;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("An error occurred while adding the amenity", e);
        }
    }

    public async Task<RoomTypeService?> UpdateRoomTypeService(RoomTypeService roomTypeService)
    {
        try
        {
            var existingRoomTypeService = GetRoomTypeServiceById(roomTypeService.ServiceId);
            if (existingRoomTypeService == null)
                throw new NullReferenceException("There is no room type service with the provided Id.");
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = roomTypeService.Id },
                new SqlParameter("@ServiceId", SqlDbType.UniqueIdentifier) { Value = roomTypeService.ServiceId },
                new SqlParameter("@RoomTypeId", SqlDbType.UniqueIdentifier) { Value = roomTypeService.RoomTypeId },
                new SqlParameter("@Amount", SqlDbType.Int) { Value = roomTypeService.Amount },
                new SqlParameter("@Status", SqlDbType.Int) { Value = roomTypeService.Status },
                new SqlParameter("@ModifiedTime", SqlDbType.DateTimeOffset) { Value = roomTypeService.ModifiedTime },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = roomTypeService.ModifiedBy }
            };
            
            await _worker.GetDataTableAsync(StoredProcedureConstant.SP_UpdateRoomTypeService, parameters);
            return await existingRoomTypeService;
        }
        catch (Exception e)
        {
            throw new Exception("An error occurred while updating the amenity room", e);
        }
    }

    public  async Task<RoomTypeService?> DeleteRoomTypeService(RoomTypeService roomTypeService)
    {
        try
        {
            var existingRoomTypeService = GetRoomTypeServiceById(roomTypeService.Id);
            if (existingRoomTypeService == null)
                throw new NullReferenceException("There is no room type service with the provided Id.");
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = roomTypeService.Id },
                new SqlParameter("@Status", SqlDbType.Int) { Value = roomTypeService.Status },
                new SqlParameter("@Deleted", SqlDbType.Bit) { Value = roomTypeService.Deleted },
                new SqlParameter("@DeletedTime", SqlDbType.DateTimeOffset) { Value = roomTypeService.DeletedTime },
                new SqlParameter("@DeletedBy", SqlDbType.UniqueIdentifier) { Value = roomTypeService.DeletedBy }
            };
            
            await _worker.GetDataTableAsync(StoredProcedureConstant.SP_DeleteRoomTypeService, parameters);
            return await existingRoomTypeService;
        }
        catch (Exception e)
        {
            throw new Exception("Some errors when deleted amenity rooom", e);
        }
    }

    private RoomTypeService ConvertDataRowToRoomTypeService(DataRow row)
    {
        return new RoomTypeService()
        {
            Id = Guid.Parse(row["Id"].ToString()!),
            ServiceId = Guid.Parse(row["ServiceId"].ToString()!),
            RoomTypeId = Guid.Parse(row["RoomTypeId"].ToString()!),
            Amount = int.Parse(row["Amount"].ToString()!),
            Status = (EntityStatus)Enum.Parse(typeof(EntityStatus), row["Status"].ToString()!),
            CreatedTime = ConvertDateTimeOffsetToString(row, "CreatedTime"),
            CreatedBy = ConvertGuidToString(row, "CreatedBy"),
            ModifiedTime = ConvertDateTimeOffsetToString(row, "ModifiedTime"),
            ModifiedBy = ConvertGuidToString(row, "ModifiedBy"),
            Deleted = row["Deleted"] != DBNull.Value && (bool)row["Deleted"],
            DeletedTime = ConvertDateTimeOffsetToString(row, "DeletedTime"),
            DeletedBy = ConvertGuidToString(row, "DeletedBy")
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
}