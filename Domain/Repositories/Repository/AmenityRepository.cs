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

    public Task<Amenity> UpdateAmenity(Amenity amenity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAmenityById(Guid amenityId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Amenity>> GetAllAmenities()
    {
        try
        {
            var amenities = new List<Amenity>();
            var dataTable = await _worker.GetDataTableAsync("SP_GetAllAmenities", null);

            foreach (DataRow row in dataTable.Rows)
            {
                var amentity = new Amenity()
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
                amenities.Add(amentity);
            }
            return amenities;
        }
        catch (Exception e)
        {
            Console.WriteLine();
            throw new Exception("An error occurred while getting all amenities", e);
        }
    }

    public Task<Amenity?> GetAmenityById(Guid amenityId)
    {
        throw new NotImplementedException();
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