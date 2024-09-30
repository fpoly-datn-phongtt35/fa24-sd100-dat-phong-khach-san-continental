using System.Data;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Utilities.StoredProcedure;

namespace Domain.Repositories.Repository;

public class RoomTypeRepository : IRoomTypeRepository
{
    private readonly DbWorker _worker;
    private readonly IConfiguration _configuration;
    
    public RoomTypeRepository(IConfiguration configuration)
    {
        _worker = new DbWorker(StoredProcedureConstant.Continetal);
        _configuration = configuration;
    }
    
    public async Task<List<RoomType>> GetAllRoomTypes()
    {
        try
        {
            var roomTypes = new List<RoomType>();
            var dataTable = await _worker.GetDataTableAsync(StoredProcedureConstant.SP_GetAllRoomTypes, null);

            foreach (DataRow row in dataTable.Rows)
            {
                var roomType = ConvertDataRowToRoomType(row);
                roomTypes.Add(roomType);
            }

            return roomTypes;
        }
        catch (Exception e)
        {
            Console.WriteLine();
            throw new Exception("An error occurred while getting all room types", e);
        }
    }

    public async Task<RoomType?> GetRoomTypeById(Guid roomTypeId)
    {
        try
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = roomTypeId }
            };
            
            var dataTable = await _worker.GetDataTableAsync(StoredProcedureConstant.SP_GetRoomTypeById, parameters);

            if (dataTable.Rows.Count == 0)
            {
                return null;
            }
            // Convert data row
            var row = dataTable.Rows[0];
            var roomType = ConvertDataRowToRoomType(row);
            
            return roomType;
        }
        catch (Exception e)
        {
            throw new ArgumentNullException("An error occurred while retrieving the room type", e);
        }
    }

    public async Task<RoomType> AddRoomType(RoomType roomType)
    {
        try
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Name", SqlDbType.NVarChar) { Value = roomType.Name },
                new SqlParameter("@Description", SqlDbType.NVarChar) { Value = roomType.Description },
                new SqlParameter("@Status", SqlDbType.Int) { Value = roomType.Status },
                new SqlParameter("@MaximumOccupancy", SqlDbType.Int) { Value = roomType.MaximumOccupancy },
                new SqlParameter("@CreatedTime", SqlDbType.DateTimeOffset) { Value = DateTimeOffset.Now },
                new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier){ Value = roomType.CreatedBy },
                new SqlParameter("@ModifiedTime", SqlDbType.DateTimeOffset) { Value = roomType.ModifiedTime },
                new SqlParameter("@Deleted", SqlDbType.Bit) { Value = roomType.Deleted },
                new SqlParameter("@DeletedTime", SqlDbType.DateTimeOffset) { Value = roomType.DeletedTime }
            };

            await _worker.GetDataTableAsync(StoredProcedureConstant.SP_InsertRoomType, parameters);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("An error occurred while adding the amenity", e);
        }

        return roomType;
    }

    public async Task<RoomType?> UpdateRoomType(RoomType roomType)
    {
        try
        {
            var existingRoomType = GetRoomTypeById(roomType.Id);
            if (existingRoomType == null)
            {
                throw new Exception("Room type could not be found");
            }

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = roomType.Id },
                new SqlParameter("@Name", SqlDbType.NVarChar) { Value = roomType.Name },
                new SqlParameter("@Description", SqlDbType.NVarChar) { Value = roomType.Description },
                new SqlParameter("@MaximumOccupancy", SqlDbType.Int) { Value = roomType.MaximumOccupancy },
                new SqlParameter("@Status", SqlDbType.Int) { Value = roomType.Status },
                new SqlParameter("@ModifiedTime", SqlDbType.DateTimeOffset) { Value = DateTimeOffset.Now },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = roomType.ModifiedBy }
            };
            
            await _worker.GetDataTableAsync(StoredProcedureConstant.SP_UpdateRoomType, parameters);
            
            return await existingRoomType;
        }
        catch (Exception e)
        {
            throw new Exception("An error occurred while updating the room type", e);
        }
    }

    public async Task<RoomType?> DeleteRoomType(RoomType roomType)
    {
        try
        {
            var existingRoomType = GetRoomTypeById(roomType.Id);
            if (existingRoomType == null)
            {
                throw new Exception("Room type could not be found");
            }
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = roomType.Id },
                new SqlParameter("@Status", SqlDbType.Int) { Value = roomType.Status },
                new SqlParameter("@Deleted", SqlDbType.Bit) { Value = roomType.Deleted },
                new SqlParameter("@DeletedTime", SqlDbType.DateTimeOffset) { Value = DateTimeOffset.Now },
                new SqlParameter("@DeletedBy", SqlDbType.UniqueIdentifier) { Value = roomType.DeletedBy },
            };
            
            await _worker.GetDataTableAsync(StoredProcedureConstant.SP_DeleteRoomType, parameters);
            
            return await existingRoomType;
        }
        catch (Exception e)
        {
            throw new Exception("Some errors when deleted room type", e);
        }
    }

    public async Task<RoomType?> RollBackDeleteRoomType(RoomType roomType)
    {
        try
        {
            var existingRoomType = GetRoomTypeById(roomType.Id);
            if (existingRoomType == null)
            {
                throw new Exception("Room type could not be found");
            }

            roomType.Deleted = false;
            roomType.DeletedTime = default(DateTimeOffset);
            roomType.DeletedBy = null;
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = roomType.Id },
                new SqlParameter("@Status", SqlDbType.Int) { Value = roomType.Status },
                new SqlParameter("@ModifiedTime", SqlDbType.DateTimeOffset) { Value = DateTimeOffset.Now },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = roomType.ModifiedBy },
                new SqlParameter("@Deleted", SqlDbType.Bit) { Value = roomType.Deleted },
                new SqlParameter("@DeletedTime", SqlDbType.DateTimeOffset) { Value = (object)roomType.DeletedTime! ?? DBNull.Value },
                new SqlParameter("@DeletedBy", SqlDbType.UniqueIdentifier) { Value = (object)roomType.DeletedBy! ?? DBNull.Value }
            };
            
            await _worker.GetDataTableAsync(StoredProcedureConstant.SP_RollBackDeletedRoomType, parameters);
            
            return await existingRoomType;
        }
        catch (Exception e)
        {
            throw new Exception("An error occurred while rolling back deleted amenity", e);
        }
    }

    private RoomType ConvertDataRowToRoomType(DataRow row)
    {
        return new RoomType()
        {
            Id = Guid.Parse(row["Id"].ToString()!),
            Name = row["Name"].ToString()!,
            Description = row["Description"].ToString()!,
            Status = (EntityStatus)Enum.Parse(typeof(EntityStatus), row["Status"].ToString()!),
            MaximumOccupancy = int.Parse(row["MaximumOccupancy"].ToString()!),
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