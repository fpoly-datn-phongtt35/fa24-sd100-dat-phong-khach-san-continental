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
                // new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = amenity.Id },
                // new SqlParameter("@Name", SqlDbType.NVarChar) { Value = amenity.Name },
                // new SqlParameter("@Description", SqlDbType.NVarChar) { Value = amenity.Description },
                // new SqlParameter("@Status", SqlDbType.Int) { Value = (int)amenity.Status },
                // new SqlParameter("@CreatedTime", SqlDbType.DateTimeOffset) { Value = amenity.CreatedTime },
                // new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier) { Value = amenity.CreatedBy },
                new SqlParameter("@Name", amenity.Name),
                new SqlParameter("@Description", amenity.Description),
                new SqlParameter("@Status", (int)amenity.Status),
                new SqlParameter("@CreatedTime", amenity.CreatedTime),
                new SqlParameter("@CreatedBy",  amenity.CreatedBy)
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

            // Executing the stored procedure to get all amenities
            var dataTable = _worker.GetDataTable("SP_GetAllAmenities");

            foreach (DataRow row in dataTable.Rows)
            {
                var description = string.Empty;
                if (description != null)
                {
                    var amenity = new Amenity
                    {
                        Id = Guid.Parse(row["Id"].ToString()!),
                        Name = row["Name"].ToString()!,
                        Description = row["Description"].ToString()!,
                        Status = (EntityStatus)Enum.Parse(typeof(EntityStatus), row["Status"].ToString()!),
                        CreatedTime = row["CreatedTime"] != DBNull.Value
                            ? DateTimeOffset.Parse(row["CreatedTime"].ToString()!)
                            : (DateTimeOffset?)null,
                        CreatedBy = row["CreatedBy"] != DBNull.Value
                            ? Guid.Parse(row["CreatedBy"].ToString()!)
                            : (Guid?)null,
                        ModifiedTime = row["ModifiedTime"] != DBNull.Value
                            ? DateTimeOffset.Parse(row["ModifiedTime"].ToString()!)
                            : (DateTimeOffset?)null,
                        ModifiedBy = row["ModifiedBy"] != DBNull.Value
                            ? Guid.Parse(row["ModifiedBy"].ToString()!)
                            : (Guid?)null,
                        Deleted = (bool)row["Deleted"],
                        DeletedBy = row["DeletedBy"] != DBNull.Value
                            ? Guid.Parse(row["DeletedBy"].ToString()!)
                            : (Guid?)null,
                        DeletedTime = row["DeletedTime"] != DBNull.Value
                            ? DateTimeOffset.Parse(row["DeletedTime"].ToString()!)
                            : (DateTimeOffset?)null
                    };

                    amenities.Add(amenity);
                }
            }

            return await Task.FromResult(amenities);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw new Exception("An error occurred while retrieving amenities", ex);
        }
    }

    public Task<Amenity?> GetAmenityById(Guid amenityId)
    {
        throw new NotImplementedException();
    }
}