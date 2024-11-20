using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.DTO.Amenity;
using Domain.DTO.Paging;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Ocsp;
using Utilities.StoredProcedure;

namespace Domain.Repositories.Repository;

public class AmenityRepository : IAmenityRepository
{
    private readonly DbWorker _worker;
    private readonly IConfiguration _configuration;

    public AmenityRepository(IConfiguration configuration)
    {
        _worker = new DbWorker(StoredProcedureConstant.Continetal);
        _configuration = configuration;
    }

    public async Task<Amenity> AddAmenity(Amenity amenity)
    {
        try
        {
            // Creating list of SQL parameters
            SqlParameter[] parameters =
            {
                new("@Name", SqlDbType.NVarChar) { Value = amenity.Name },
                new("@Description", SqlDbType.NVarChar) { Value = amenity.Description },
                new("@Status", SqlDbType.Int) { Value = amenity.Status },
                new("@CreatedTime", SqlDbType.DateTimeOffset) { Value = DateTimeOffset.Now },
                new("@CreatedBy", SqlDbType.UniqueIdentifier) { Value = amenity.CreatedBy },
                new("@NewAmenityId", SqlDbType.UniqueIdentifier) { Direction = ParameterDirection.Output }
            };

            // Executing the stored procedure to insert data
            await _worker.GetDataTableAsync(StoredProcedureConstant.SP_InsertAmenity, parameters);
            
            amenity.Id = (Guid)parameters[5].Value;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw new Exception("An error occurred while adding the amenity", ex);
        }

        return amenity;
    }

    public async Task<Amenity?> UpdateAmenity(Amenity amenity)
    {
        try
        {
            var existingAmenity = GetAmenityById(amenity.Id);
            if (existingAmenity == null)
            {
                throw new Exception("Amenity not found");
            }

            SqlParameter[] parameters = new SqlParameter[]
            {
                new("@Id", SqlDbType.UniqueIdentifier) { Value = amenity.Id },
                new("@Name", SqlDbType.NVarChar) { Value = amenity.Name },
                new("@Description", SqlDbType.NVarChar) { Value = amenity.Description },
                new("@Status", SqlDbType.Int) { Value = amenity.Status },
                new("@ModifiedTime", SqlDbType.DateTimeOffset) { Value = DateTimeOffset.Now },
                new("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = amenity.ModifiedBy },
                new("@Deleted", SqlDbType.Bit) {Value = amenity.Deleted}
            };

            await _worker.ExecuteNonQueryAsync(StoredProcedureConstant.SP_UpdateAmenity, parameters);

            return await existingAmenity;
        }
        catch (Exception e)
        {
            throw new Exception("An error occurred while updating the amenity", e);
        }
    }

    public async Task<Amenity?> DeleteAmenityById(Amenity amenity)
    {
        try
        {
            var existingAmenity = GetAmenityById(amenity.Id);
            if (existingAmenity == null)
            {
                throw new Exception("Amenity not found");
            }

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = amenity.Id },
                new SqlParameter("@Status", SqlDbType.Int) { Value = amenity.Status },
                new SqlParameter("@Deleted", SqlDbType.Bit) { Value = amenity.Deleted },
                new SqlParameter("@DeletedTime", SqlDbType.DateTimeOffset) { Value = DateTimeOffset.Now },
                new SqlParameter("@DeletedBy", SqlDbType.UniqueIdentifier) { Value = amenity.DeletedBy },
            };

            await _worker.GetDataTableAsync(StoredProcedureConstant.SP_DeleteAmenity, parameters);

            return await existingAmenity;
        }
        catch (Exception e)
        {
            throw new Exception("Some errors when deleted amenity", e);
        }
    }

    public async Task<Amenity?> GetAmenityById(Guid amenityId)
    {
        try
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = amenityId }
            };

            // Get data from Stored Procedure
            var dataTable = await _worker.GetDataTableAsync(StoredProcedureConstant.SP_GetAmenityById, parameters);

            if (dataTable.Rows.Count == 0)
            {
                return null;
            }

            // Convert data row 
            var row = dataTable.Rows[0];
            var amenity = ConvertDataRowToAmenity(row);

            return amenity;
        }
        catch (Exception e)
        {
            throw new ArgumentNullException("An error occurred while retrieving the amenity", e);
        }
    }

    public async Task<ResponseData<AmenityResponse>> GetFilteredDeletedAmenity(AmenityGetRequest amenityGetRequest)
    {
        var model = new ResponseData<AmenityResponse>();
        try
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new("@PageIndex", amenityGetRequest.PageIndex),
                new("@PageSize", amenityGetRequest.PageSize),
                new("@SearchString", amenityGetRequest.SearchString),
                new("@Status", amenityGetRequest.Status)
            };
            
            var dataTable = await _worker.GetDataTableAsync
                (StoredProcedureConstant.SP_GetFilteredDeletedAmenities, parameters);
            var deletedAmenities = new List<AmenityResponse>();

            foreach (DataRow row in dataTable.Rows)
            {
                var deletedAmenity = ConvertDataRowToAmenity(row);
                var deletedAmenityResponse = deletedAmenity.ToAmenityResponse();
                deletedAmenities.Add(deletedAmenityResponse);
            }
            // Gan list amenities vao model
            model.data = deletedAmenities;

            model.CurrentPage = amenityGetRequest.PageIndex;
            model.PageSize = amenityGetRequest.PageSize;

            try
            {
                model.totalRecord = Convert.ToInt32(dataTable.Rows[0]["TotalRows"]);
            }
            catch (Exception e)
            {
                model.totalRecord = 0;
            }

            //tong trang
            model.totalPage = (int)Math.Ceiling((double)model.totalRecord / amenityGetRequest.PageSize);
        }
        catch (Exception e)
        {
            Console.WriteLine();
            throw new Exception("An error occurred while getting all deleted amenities", e);
        }

        return model;
    }

    public async Task<Amenity?> RecoverDeletedAmenity(Amenity amenity)
    {
        try
        {
            var existingAmenity = GetAmenityById(amenity.Id);
            if (existingAmenity == null)
            {
                throw new Exception("Amenity not found");
            }

            SqlParameter[] parameters = new SqlParameter[]
            {
                new("@Id", SqlDbType.UniqueIdentifier) { Value = amenity.Id },
                new("@Status", SqlDbType.Int) { Value = amenity.Status },
                new("@ModifiedTime", SqlDbType.DateTimeOffset) { Value = DateTimeOffset.Now },
                new("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = amenity.ModifiedBy },
                new("@Deleted", SqlDbType.Bit) { Value = amenity.Deleted },
                new("@DeletedTime", SqlDbType.DateTimeOffset) { Value = DBNull.Value },
                new("@DeletedBy", SqlDbType.UniqueIdentifier) { Value = DBNull.Value },
            };

            await _worker.GetDataTableAsync(StoredProcedureConstant.SP_RecoverDeletedAmenity, parameters);
            return await existingAmenity;
        }
        catch (Exception e)
        {
            throw new Exception("An error occurred while recovering deleted amenity", e);
        }
    }

    public async Task<ResponseData<AmenityResponse>> GetFilteredAmenities(AmenityGetRequest amenityGetRequest)
    {
        var model = new ResponseData<AmenityResponse>();
        try
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new("@PageSize", amenityGetRequest.PageSize),
                new("@PageIndex", amenityGetRequest.PageIndex),
                new("@SearchString", amenityGetRequest.SearchString),
                new("@Status", amenityGetRequest.Status)
            };

            var dataTable = await _worker.GetDataTableAsync
                (StoredProcedureConstant.SP_GetFilteredAmenities, parameters);
            var amenities = new List<AmenityResponse>();
            foreach (DataRow row in dataTable.Rows)
            {
                var amenity = ConvertDataRowToAmenity(row);
                var amenityResponse = amenity.ToAmenityResponse();
                amenities.Add(amenityResponse);
            }

            // Gan list amenities vao model
            model.data = amenities;

            model.CurrentPage = amenityGetRequest.PageIndex;
            model.PageSize = amenityGetRequest.PageSize;

            try
            {
                model.totalRecord = Convert.ToInt32(dataTable.Rows[0]["TotalRows"]);
            }
            catch (Exception e)
            {
                model.totalRecord = 0;
            }

            //tong trang
            model.totalPage = (int)Math.Ceiling((double)model.totalRecord / amenityGetRequest.PageSize);
        }
        catch (Exception e)
        {
            throw new Exception("An error occurred while retrieving filtered amenities", e);
        }

        return model;
    }

    public Amenity ConvertDataRowToAmenity(DataRow row)
    {
        return new Amenity()
        {
            Id = Guid.Parse(row["Id"].ToString()!),
            Name = row["Name"].ToString()!,
            Description = row["Description"].ToString()!,
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