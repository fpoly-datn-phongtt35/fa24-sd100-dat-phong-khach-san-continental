using System.Data;
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
                new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = amenity.Id },
                new SqlParameter("@Name", SqlDbType.NVarChar) { Value = amenity.Name },
                new SqlParameter("@Description", SqlDbType.NVarChar) { Value = amenity.Description },
                new SqlParameter("@Status", SqlDbType.Int) { Value = (int)amenity.Status },
                new SqlParameter("@CreatedTime", SqlDbType.DateTimeOffset) { Value = amenity.CreatedTime },
                new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier) { Value = amenity.CreatedBy }
            };

            // Executing the stored procedure to insert data
            await Task.Run(() => _worker.ExecuteNonQuery("AddAmenityProc", parameters));

            return amenity;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw new Exception("An error occurred while adding the amenity", ex);
        }
    }

    public Task<Amenity> UpdateAmenity(Amenity amenity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAmenityById(Guid amenityId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Amenity>> GetAllAmenities()
    {
        throw new NotImplementedException();
    }

    public Task<Amenity?> GetAmenityById(Guid amenityId)
    {
        throw new NotImplementedException();
    }
}