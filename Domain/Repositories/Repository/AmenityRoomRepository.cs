using System.Data;
using System.Linq.Expressions;
using Domain.DTO.AmenityRoom;
using Domain.DTO.Paging;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Utilities.StoredProcedure;

namespace Domain.Repositories.Repository;

public class AmenityRoomRepository : IAmenityRoomRepository
{
    private readonly DbWorker _worker;
    private readonly IConfiguration _configuration;

    public AmenityRoomRepository(IConfiguration configuration)
    {
        _worker = new DbWorker(StoredProcedureConstant.Continetal);
        _configuration = configuration;
    }

    public async Task<ResponseData<AmenityRoomResponse>> GetFilteredAmenityRooms(AmenityRoomGetRequest amenityRoomGetRequest)
    {
        var model = new ResponseData<AmenityRoomResponse>();
        try
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new("@PageIndex", amenityRoomGetRequest.PageIndex),
                new("@PageSize", amenityRoomGetRequest.PageSize),
                new("@SearchString", amenityRoomGetRequest.SearchString),
                new("@Status", amenityRoomGetRequest.Status),
                new("@RoomTypeId", amenityRoomGetRequest.RoomTypeId)
            };
            
            var dataTable = await _worker.GetDataTableAsync
                (StoredProcedureConstant.SP_GetFilteredAmenityRooms, parameters);
            var amenityRooms = new List<AmenityRoomResponse>();
            
            foreach (DataRow row in dataTable.Rows)
            {
                var amenityRoom = ConvertDataRowToAmenityRoom(row);
                var amenityRoomResponse = amenityRoom.ToAmenityRoomResponse();
                amenityRooms.Add(amenityRoomResponse);
            }

            model.data = amenityRooms;
            model.CurrentPage = amenityRoomGetRequest.PageIndex;
            model.PageSize = amenityRoomGetRequest.PageSize;
            
            try
            {
                model.totalRecord = Convert.ToInt32(dataTable.Rows[0]["TotalRows"]);
            }
            catch (Exception e)
            {
                model.totalRecord = 0;
            }

            model.totalPage = (int)Math.Ceiling((double)model.totalRecord / amenityRoomGetRequest.PageSize);
        }
        catch (Exception e)
        {
            throw new ArgumentNullException("An error occurred while getting all room types", e);
        }

        return model;
    }

    public async Task<AmenityRoom?> GetAmenityRoomById(Guid amenityRoomId)
    {
        try
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = amenityRoomId }
            };

            var dataTable = await _worker.GetDataTableAsync(StoredProcedureConstant.SP_GetAmenityRoomById, parameters);

            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];
            var amenityRoom = ConvertDataRowToAmenityRoom(row);

            return amenityRoom;
        }
        catch (Exception e)
        {
            throw new ArgumentNullException("An error occurred while retrieving the amenity room", e);
        }
    }

    public async Task<AmenityRoom> AddAmenityRoom(AmenityRoom amenityRoom)
    {
        try
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@AmenityId", SqlDbType.UniqueIdentifier) { Value = amenityRoom.AmenityId },
                new SqlParameter("@RoomTypeId", SqlDbType.UniqueIdentifier) { Value = amenityRoom.RoomTypeId },
                new SqlParameter("@Amount", SqlDbType.Int) { Value = amenityRoom.Amount },
                new SqlParameter("@Status", SqlDbType.Int) { Value = amenityRoom.Status },
                new SqlParameter("@CreatedTime", SqlDbType.DateTimeOffset) { Value = DateTimeOffset.Now },
                new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier) { Value = amenityRoom.CreatedBy },
                new SqlParameter("@ModifiedTime", SqlDbType.DateTimeOffset) { Value = amenityRoom.ModifiedTime },
                new SqlParameter("@Deleted", SqlDbType.Bit) { Value = amenityRoom.Deleted },
                new SqlParameter("@DeletedTime", SqlDbType.DateTimeOffset) { Value = amenityRoom.DeletedTime }
            };

            await _worker.GetDataTableAsync(StoredProcedureConstant.SP_InsertAmenityRoom, parameters);

            return amenityRoom;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("An error occurred while adding the amenity", e);
        }
    }

    public async Task<AmenityRoom?> UpdateAmenityRoom(AmenityRoom amenityRoom)
    {
        try
        {
            var existingAmenityRoom = GetAmenityRoomById(amenityRoom.Id);
            if (existingAmenityRoom == null)
            {
                throw new Exception("There is no amenity room with the provided Id.");
            }

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = amenityRoom.Id },
                new SqlParameter("@AmenityId", SqlDbType.UniqueIdentifier) { Value = amenityRoom.AmenityId },
                new SqlParameter("@RoomTypeId", SqlDbType.UniqueIdentifier) { Value = amenityRoom.RoomTypeId },
                new SqlParameter("@Amount", SqlDbType.Int) { Value = amenityRoom.Amount },
                new SqlParameter("@Status", SqlDbType.Int) { Value = amenityRoom.Status },
                new SqlParameter("@ModifiedTime", SqlDbType.DateTimeOffset) { Value = DateTimeOffset.Now },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = amenityRoom.ModifiedBy }
            };
            await _worker.GetDataTableAsync(StoredProcedureConstant.SP_UpdateAmenityRoom, parameters);

            return await existingAmenityRoom;
        }
        catch (Exception e)
        {
            throw new Exception("An error occurred while updating the amenity room", e);
        }
    }

    public async Task<AmenityRoom?> DeleteAmenityRoom(AmenityRoom amenityRoom)
    {
        try
        {
            var existingAmenityRoom = GetAmenityRoomById(amenityRoom.Id);
            if (existingAmenityRoom == null)
            {
                throw new Exception("There is no amenity room with the provided Id.");
            }

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = amenityRoom.Id },
                new SqlParameter("@Status", SqlDbType.Int) { Value = amenityRoom.Status },
                new SqlParameter("@Deleted", SqlDbType.Bit) { Value = amenityRoom.Deleted },
                new SqlParameter("@DeletedTime", SqlDbType.DateTimeOffset) { Value = DateTimeOffset.Now },
                new SqlParameter("@DeletedBy", SqlDbType.UniqueIdentifier) { Value = amenityRoom.DeletedBy },
            };

            await _worker.GetDataTableAsync(StoredProcedureConstant.SP_DeleteAmenityRoom, parameters);

            return await existingAmenityRoom;
        }
        catch (Exception e)
        {
            throw new Exception("Some errors when deleted amenity rooom", e);
        }
    }

    public async Task<ResponseData<AmenityRoomResponse>> GetFilteredDeletedAmenityRooms
        (AmenityRoomGetRequest amenityRoomGetRequest)
    {
        var model = new ResponseData<AmenityRoomResponse>();
        try
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new("@PageIndex", amenityRoomGetRequest.PageIndex),
                new("@PageSize", amenityRoomGetRequest.PageSize),
                new("@SearchString", amenityRoomGetRequest.SearchString),
                new("@Status", amenityRoomGetRequest.Status),
                new("@RoomTypeId", amenityRoomGetRequest.RoomTypeId)
            };
            
            var dataTable = await _worker.GetDataTableAsync
                (StoredProcedureConstant.SP_GetFilteredDeletedAmenityRooms, parameters);
            var deletedAmenityRooms = new List<AmenityRoomResponse>();
            
            foreach (DataRow row in dataTable.Rows)
            {
                var deletedAmenityRoom = ConvertDataRowToAmenityRoom(row);
                var deletedAmenityRoomResponse = deletedAmenityRoom.ToAmenityRoomResponse();
                deletedAmenityRooms.Add(deletedAmenityRoomResponse);
            }
            
            model.data = deletedAmenityRooms;
            model.CurrentPage = amenityRoomGetRequest.PageIndex;
            model.PageSize = amenityRoomGetRequest.PageSize;

            try
            {
                model.totalRecord = Convert.ToInt32(dataTable.Rows[0]["TotalRows"]);
            }
            catch (Exception e)
            {
                model.totalRecord = 0;
            }
            model.totalPage = (int)Math.Ceiling((double)model.totalRecord / amenityRoomGetRequest.PageSize);
        }
        catch (Exception e)
        {
            Console.WriteLine();
            throw new Exception("An error occurred while getting all deleted amenity rooms", e);
        }

        return model;
    }

    public async Task<AmenityRoom?> RecoverDeletedAmenityRoom(AmenityRoom amenityRoom)
    {
        try
        {
            var existingAmenityRoom = GetAmenityRoomById(amenityRoom.Id);
            if(existingAmenityRoom == null)
                throw new Exception("There is no amenity room with the provided Id.");

            SqlParameter[] parameters = new SqlParameter[]
            {
                new("@Id", SqlDbType.UniqueIdentifier) { Value = amenityRoom.Id },
                new("@Status", SqlDbType.Int) { Value = amenityRoom.Status },
                new("@ModifiedTime", SqlDbType.DateTimeOffset) { Value = DateTimeOffset.Now },
                new("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = amenityRoom.ModifiedBy },
                new("@Deleted", SqlDbType.Bit) { Value = amenityRoom.Deleted },
                new("@DeletedTime", SqlDbType.DateTimeOffset) { Value = DateTimeOffset.MinValue },
                new("@DeletedBy", SqlDbType.UniqueIdentifier) { Value = DBNull.Value }
            };
            
            await _worker.GetDataTableAsync
                (StoredProcedureConstant.SP_RecoverDeletedAmenityRoom, parameters);
            return await existingAmenityRoom;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("An error occurred while recovering deleted amenity room", e);
        }
    }

    private AmenityRoom ConvertDataRowToAmenityRoom(DataRow row)
    {
        return new AmenityRoom()
        {
            Id = Guid.Parse(row["Id"].ToString()!),
            AmenityId = Guid.Parse(row["AmenityId"].ToString()!),
            RoomTypeId = Guid.Parse(row["RoomTypeId"].ToString()!),
            Amount = int.Parse(row["Amount"].ToString()!),
            Status = (EntityStatus)Enum.Parse(typeof(EntityStatus), row["Status"].ToString()!),
            CreatedTime = ConvertDateTimeOffsetToString(row, "CreatedTime"),
            CreatedBy = ConvertGuidToString(row, "CreatedBy"),
            ModifiedTime = ConvertDateTimeOffsetToString(row, "ModifiedTime"),
            ModifiedBy = ConvertGuidToString(row, "ModifiedBy"),
            Deleted = row["Deleted"] != DBNull.Value && (bool)row["Deleted"],
            DeletedTime = ConvertDateTimeOffsetToString(row, "DeletedTime"),
            DeletedBy = ConvertGuidToString(row, "DeletedBy"),
            Amenity = new Amenity 
            { 
                Id = (Guid)row["AmenityId"], 
                Name = (string)row["AmenityName"] 
            },
            RoomType = new RoomType 
            { 
                Id = (Guid)row["RoomTypeId"], 
                Name = (string)row["RoomTypeName"] 
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
}