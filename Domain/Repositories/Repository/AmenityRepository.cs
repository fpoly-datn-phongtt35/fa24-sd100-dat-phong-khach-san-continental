using System.Data;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
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
                new SqlParameter("@Name", amenity.Name),
                new SqlParameter("@Description", amenity.Description),
                new SqlParameter("@Status", (int)amenity.Status),
                new SqlParameter("@CreatedTime", amenity.CreatedTime),
                new SqlParameter("@CreatedBy", amenity.CreatedBy)
            };

            // Executing the stored procedure to insert data
            await _worker.GetDataTableAsync(StoredProcedureConstant.SP_InsertAmenity, parameters);
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
                new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = amenity.Id },
                new SqlParameter("@Name", SqlDbType.NVarChar) { Value = amenity.Name },
                new SqlParameter("@Description", SqlDbType.NVarChar) { Value = amenity.Description },
                new SqlParameter("@Status", SqlDbType.Int) { Value = amenity.Status },
                new SqlParameter("@ModifiedTime", SqlDbType.DateTimeOffset) { Value = DateTimeOffset.Now },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = amenity.ModifiedBy }
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
                new SqlParameter("@DeletedBy", SqlDbType.UniqueIdentifier) { Value = amenity.ModifiedBy },
            };
            
            await _worker.GetDataTableAsync(StoredProcedureConstant.SP_DeleteAmenity, parameters);

            return await existingAmenity;
        }
        catch (Exception e)
        {
            throw new Exception("Some errors when deleted amenity", e);
        }
    }

    public async Task<List<Amenity>> GetAllAmenities()
    {
        try
        {
            var amenities = new List<Amenity>();
            var dataTable = await _worker.GetDataTableAsync(StoredProcedureConstant.SP_GetAllAmenities, null);

            foreach (DataRow row in dataTable.Rows)
            {
                var amenity = ConvertDataRowToAmenity(row);
                amenities.Add(amenity);
            }

            return amenities;
        }
        catch (Exception e)
        {
            Console.WriteLine();
            throw new Exception("An error occurred while getting all amenities", e);
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

    public static DateTimeOffset? ConvertDateTimeOffsetToString(DataRow row, string columnName)
    {
        if (row[columnName] != DBNull.Value)
        {
            return DateTimeOffset.Parse(row[columnName].ToString()!);
        }

        return null;
    }

    public static Guid? ConvertGuidToString(DataRow row, string columnName)
    {
        if (row[columnName] != DBNull.Value)
        {
            return Guid.Parse(row[columnName].ToString()!);
        }

        return null;
    }
}