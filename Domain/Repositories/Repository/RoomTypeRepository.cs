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
    
    public async Task<List<RoomType>> GetAllRoomTypes(string? search)
    {
        try
        {
            var roomTypes = new List<RoomType>();
            SqlParameter[] parameters = null;

            if (!string.IsNullOrEmpty(search))
            {
                parameters = new SqlParameter[]
                {
                    new SqlParameter("@search", SqlDbType.NVarChar) { Value = $"%{search}%" },
                };
            }
            
            var dataTable = await _worker.GetDataTableAsync
                (!string.IsNullOrEmpty(search) ? 
                    StoredProcedureConstant.SP_GetAllRoomTypesWithSearch :
                    StoredProcedureConstant.SP_GetAllRoomTypes,
                    parameters);

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

    public async Task<RoomType?> GetRoomTypeWithAmenityRoomsAndRoomTypeServicesById(Guid roomTypeId)
    {
        try
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = roomTypeId }
            };
            
            var roomTypeDataTable = await _worker.GetDataTableAsync
                (StoredProcedureConstant.SP_GetRoomTypeById, parameters);
            if (roomTypeDataTable.Rows.Count == 0)
                return null;
            
            var row = roomTypeDataTable.Rows[0];
            var roomType = ConvertDataRowToRoomType(row);
            
            // Get List AmenityRooms to RoomType
            roomType.AmenityRooms = await GetAmenityRoomsByRoomTypeId(roomTypeId);
            
            // Get List RoomTypeServices to RoomType
            roomType.RoomsTypeServices = await GetRoomTypeServicesByRoomTypeId(roomTypeId);
            
            return roomType;
        }
        catch (Exception e)
        {
            throw new Exception("An error occurred while retrieving the room type with amenity rooms and services", e);
        }
    }

    public async Task<List<AmenityRoom>> GetAmenityRoomsByRoomTypeId(Guid roomTypeId)
    {
        try
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@RoomTypeId", SqlDbType.UniqueIdentifier) { Value = roomTypeId }
            };
        
            var dataTable = await _worker.GetDataTableAsync
                (StoredProcedureConstant.SP_GetAmenityRoomsByRoomTypeId, parameters);
            if (dataTable.Rows.Count == 0)
            {
                return new List<AmenityRoom>(); // Không có dữ liệu trả về
            }
            var amenityRooms = new List<AmenityRoom>();

            foreach (DataRow row in dataTable.Rows)
            {
                if (dataTable.Columns.Contains("Id"))
                {
                    var amenityRoom = new AmenityRoom()
                    {
                        Id = (Guid)row["Id"],
                        RoomTypeId = (Guid)row["RoomTypeId"],
                        AmenityId = (Guid)row["AmenityId"],
                        Amount = (int)row["Amount"],
                        Status = (EntityStatus)row["Status"],
                        // Gọi thêm GetAmenityById để lấy thông tin về Amenity
                        Amenity = await GetAmenityById((Guid)row["AmenityId"])
                    };
                    amenityRooms.Add(amenityRoom);    
                }
                else
                {
                    throw new ArgumentException("Column 'Id' does not exist in the result set.");
                }
            }
        
            return amenityRooms;
        }
        catch (Exception e)
        {
            throw new Exception("An error occurred while retrieving the list amenity room", e);
        }
    }

    public async Task<List<RoomTypeService>> GetRoomTypeServicesByRoomTypeId(Guid roomTypeId)
    {
        try
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@RoomTypeId", SqlDbType.UniqueIdentifier) { Value = roomTypeId }
            };
            
            var dataTable = await _worker.GetDataTableAsync
            (StoredProcedureConstant.SP_GetRoomTypeServicesByRoomTypeId, parameters);
            if(dataTable.Rows.Count == 0)
                return new List<RoomTypeService>(); // No data return
            
            var roomTypeServices = new List<RoomTypeService>();
            foreach (DataRow row in dataTable.Rows)
            {
                var roomTypeService = new RoomTypeService()
                {
                    Id = (Guid)row["Id"],
                    RoomTypeId = (Guid)row["RoomTypeId"],
                    ServiceId = (Guid)row["ServiceId"],
                    Amount = (int)row["Amount"],
                    Status = (EntityStatus)row["Status"],
                    Service = await GetServiceById((Guid)row["ServiceId"])
                };
                roomTypeServices.Add(roomTypeService);
            }

            return roomTypeServices;
        }
        catch (Exception e)
        {
            throw new Exception("An error occurred while retrieving the room type services", e);
        }
    }

    private async Task<Amenity?> GetAmenityById(Guid amenityId)
    {
        try
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = amenityId }
            };

            var dataTable = await _worker.GetDataTableAsync
                (StoredProcedureConstant.SP_GetAmenityById, parameters);

            if (dataTable.Rows.Count == 0) return null;

            var row = dataTable.Rows[0];
            var amenity = new Amenity
            {
                Id = (Guid)row["Id"],
                Name = (string)row["Name"]
            };

            return amenity;
        }
        catch (Exception e)
        {
            throw new ArgumentNullException("An error occurred while retrieving the amenity", e);
        }
    }
    
    private async Task<Service?> GetServiceById(Guid serviceId)
    {
        try
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = serviceId }
            };

            var dataTable = await _worker.GetDataTableAsync
                (StoredProcedureConstant.SP_GetServiceById, parameters);

            if (dataTable.Rows.Count == 0) return null;

            var row = dataTable.Rows[0];
            var service = new Service()
            {
                Id = (Guid)row["Id"],
                Name = (string)row["Name"]
            };

            return service;
        }
        catch (Exception e)
        {
            throw new ArgumentNullException("An error occurred while retrieving the service", e);
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