using System.Data;
using Domain.DTO.Amenity;
using Domain.DTO.Paging;
using Domain.DTO.RoomType;
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

    public async Task<ResponseData<RoomTypeResponse>> GetFilteredRoomTypes(RoomTypeGetRequest roomTypeGetRequest)
    {
        var model = new ResponseData<RoomTypeResponse>();
        try
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new("@PageIndex", roomTypeGetRequest.PageIndex),
                new("@PageSize", roomTypeGetRequest.PageSize),
                new("@SearchString", roomTypeGetRequest.SearchString),
                new("@Status", roomTypeGetRequest.Status)
            };
            
            var dataTable = await _worker.GetDataTableAsync
                (StoredProcedureConstant.SP_GetFilteredRoomTypes, parameters);
            var roomTypes = new List<RoomTypeResponse>();
            
            foreach (DataRow row in dataTable.Rows)
            {
                var roomType = ConvertDataRowToRoomType(row);
                var roomTypeResponse = roomType.ToRoomTypeResponse();
                roomTypes.Add(roomTypeResponse);
            }

            model.data = roomTypes;
            model.CurrentPage = roomTypeGetRequest.PageIndex;
            model.PageSize = roomTypeGetRequest.PageSize;

            try
            {
                model.totalRecord = Convert.ToInt32(dataTable.Rows[0]["TotalRows"]);
            }
            catch (Exception e)
            {
                model.totalRecord = 0;
            }
            model.totalPage = (int)Math.Ceiling((double)model.totalRecord / roomTypeGetRequest.PageSize);
        }
        catch (Exception e)
        {
            Console.WriteLine();
            throw new Exception("An error occurred while getting all room types", e);
        }

        return model;
    }

    public async Task<RoomType?> GetRoomTypeById(Guid roomTypeId)
    {
        try
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new("@Id", SqlDbType.UniqueIdentifier) { Value = roomTypeId }
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
                new("@Id", SqlDbType.UniqueIdentifier) { Value = roomTypeId }
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
                new("@RoomTypeId", SqlDbType.UniqueIdentifier) { Value = roomTypeId }
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
                new("@RoomTypeId", SqlDbType.UniqueIdentifier) { Value = roomTypeId }
            };

            var dataTable = await _worker.GetDataTableAsync
                (StoredProcedureConstant.SP_GetRoomTypeServicesByRoomTypeId, parameters);
            if (dataTable.Rows.Count == 0)
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
                new("@Id", SqlDbType.UniqueIdentifier) { Value = amenityId }
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
                new("@Id", SqlDbType.UniqueIdentifier) { Value = serviceId }
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
                new("@Name", SqlDbType.NVarChar) { Value = roomType.Name },
                new("@Description", SqlDbType.NVarChar) { Value = roomType.Description ?? (object)DBNull.Value },
                new("@MaximumOccupancy", SqlDbType.Int) { Value = roomType.MaximumOccupancy },
                new("@Status", SqlDbType.Int) { Value = roomType.Status },
                new("@Quantity", SqlDbType.Int) { Value = roomType.Quantity },
                new("@CreatedTime", SqlDbType.DateTimeOffset) { Value = DateTimeOffset.Now },
                new("@CreatedBy", SqlDbType.UniqueIdentifier) { Value = roomType.CreatedBy },
                new("@Deleted", SqlDbType.Bit) { Value = roomType.Deleted },
                new("@NewRoomTypeId", SqlDbType.UniqueIdentifier) {Direction = ParameterDirection.Output}
            };

            await _worker.GetDataTableAsync(StoredProcedureConstant.SP_InsertRoomType, parameters);

            roomType.Id = (Guid)parameters[8].Value;
            return roomType;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
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
                new("@Id", SqlDbType.UniqueIdentifier) { Value = roomType.Id },
                new("@Name", SqlDbType.NVarChar) { Value = roomType.Name },
                new("@Description", SqlDbType.NVarChar) { Value = roomType.Description },
                new("@MaximumOccupancy", SqlDbType.Int) { Value = roomType.MaximumOccupancy },
                new("@Status", SqlDbType.Int) { Value = roomType.Status },
                new("@ModifiedTime", SqlDbType.DateTimeOffset) { Value = DateTimeOffset.Now },
                new("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = roomType.ModifiedBy },
                new("@Deleted", SqlDbType.Bit) {Value = roomType.Deleted}
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
                new("@Id", SqlDbType.UniqueIdentifier) { Value = roomType.Id },
                new("@Status", SqlDbType.Int) { Value = roomType.Status },
                new("@Deleted", SqlDbType.Bit) { Value = roomType.Deleted },
                new("@DeletedTime", SqlDbType.DateTimeOffset) { Value = DateTimeOffset.Now },
                new("@DeletedBy", SqlDbType.UniqueIdentifier) { Value = roomType.DeletedBy },
            };

            await _worker.GetDataTableAsync(StoredProcedureConstant.SP_DeleteRoomType, parameters);

            return await existingRoomType;
        }
        catch (Exception e)
        {
            throw new Exception("Some errors when deleted room type", e);
        }
    }

    public async Task<ResponseData<RoomTypeResponse>> GetFilteredDeletedRoomTypes(RoomTypeGetRequest roomTypeGetRequest)
    {
        var model = new ResponseData<RoomTypeResponse>();
        try
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new("@PageIndex", roomTypeGetRequest.PageIndex),
                new("@PageSize", roomTypeGetRequest.PageSize),
                new("@SearchString", roomTypeGetRequest.SearchString),
                new("@Status", roomTypeGetRequest.Status)
            };
            
            var dataTable = await _worker.GetDataTableAsync
                (StoredProcedureConstant.SP_GetFilteredDeletedRoomTypes, parameters);
            var deletedRoomTypes = new List<RoomTypeResponse>();

            foreach (DataRow row in dataTable.Rows)
            {
                var roomType = ConvertDataRowToRoomType(row);
                var deletedRoomTypeResponse = roomType.ToRoomTypeResponse();
                deletedRoomTypes.Add(deletedRoomTypeResponse);
            }
            
            model.data = deletedRoomTypes;
            model.CurrentPage = roomTypeGetRequest.PageIndex;
            model.PageSize = roomTypeGetRequest.PageSize;

            try
            {
                model.totalRecord = Convert.ToInt32(dataTable.Rows[0]["TotalRows"]);
            }
            catch (Exception e)
            {
                model.totalRecord = 0;
            }
            model.totalPage = (int)Math.Ceiling((double)model.totalRecord / roomTypeGetRequest.PageSize);
        }
        catch (Exception e)
        {
            Console.WriteLine();
            throw new Exception("Some errors while getting all deleted room types", e);
        }

        return model;
    }

    public async Task<RoomType?> RecoverDeletedRoomType(RoomType roomType)
    {
        try
        {
            var existingRoomType = GetRoomTypeById(roomType.Id);
            if (existingRoomType == null)
                throw new Exception("Room type could not be found");

            SqlParameter[] parameters = new SqlParameter[]
            {
                new("@Id", SqlDbType.UniqueIdentifier) { Value = roomType.Id },
                new("@Status", SqlDbType.Int) { Value = roomType.Status },
                new("@ModifiedTime", SqlDbType.DateTimeOffset) { Value = DateTimeOffset.Now },
                new("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = roomType.ModifiedBy },
                new("@Deleted", SqlDbType.Bit) { Value = roomType.Deleted },
                new("@DeletedTime", SqlDbType.DateTimeOffset) { Value = roomType.DeletedTime ?? (object)DBNull.Value },
                new("@DeletedBy", SqlDbType.UniqueIdentifier) { Value = default },
            };
            await _worker.GetDataTableAsync
                (StoredProcedureConstant.SP_RecoverDeletedRoomType, parameters);
            return await existingRoomType;
        }
        catch (Exception e)
        {
            Console.WriteLine();
            throw new Exception("An error occurred while recovering deleted room type", e);
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
            DeletedBy = ConvertGuidToString(row, "DeletedBy"),
            Quantity = int.Parse(row["Quantity"].ToString()!)
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