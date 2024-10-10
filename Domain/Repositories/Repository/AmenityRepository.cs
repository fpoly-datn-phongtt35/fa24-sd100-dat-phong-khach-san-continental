using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
                new SqlParameter("@Name", SqlDbType.NVarChar) { Value = amenity.Name },
                new SqlParameter("@Description", SqlDbType.NVarChar) { Value = amenity.Description },
                new SqlParameter("@Status", SqlDbType.Int) { Value = amenity.Status },
                new SqlParameter("@CreatedTime", SqlDbType.DateTimeOffset) { Value = DateTimeOffset.Now },
                new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier) { Value = amenity.CreatedBy }
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

    public async Task<List<Amenity>> GetFilteredDeletedAmenity(string? searchString)
    {
        try
        {
            var deletedAmenities = new List<Amenity>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new("@searchString", SqlDbType.NVarChar) { Value = (object)searchString ?? DBNull.Value },
            };
            var dataTable = await _worker.GetDataTableAsync
                (StoredProcedureConstant.SP_GetFilteredDeletedAmenities, parameters);

            foreach (DataRow row in dataTable.Rows)
            {
                var deletedAmenity = ConvertDataRowToAmenity(row);
                deletedAmenities.Add(deletedAmenity);
            }

            return deletedAmenities;
        }
        catch (Exception e)
        {
            Console.WriteLine();
            throw new Exception("An error occurred while getting all deleted amenities", e);
        }
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

    public async Task<List<Amenity>> GetFilteredAmenities(EntityStatus? status, string? searchString)
    {
        try
        {
            var amenities = new List<Amenity>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new("@searchString", SqlDbType.NVarChar) { Value = (object)searchString ?? DBNull.Value },
                new("@status", SqlDbType.Int) { Value = status.HasValue ? (int)status.Value : DBNull.Value }
            };
            
            var dataTable = await _worker.GetDataTableAsync
                (StoredProcedureConstant.SP_GetFilteredAmenities, parameters);
            foreach (DataRow row in dataTable.Rows)
            {
                var amenity = ConvertDataRowToAmenity(row);
                amenities.Add(amenity);
            }
            return amenities;
        }
        catch (Exception e)
        {
            throw new Exception("An error occurred while retrieving filtered amenities", e);
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

    public string GenerateToken()
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:PrivateKey"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", "1"), new Claim(ClaimTypes.Role, "User") }),
            Expires = DateTime.UtcNow.AddMinutes(15),
            Issuer = _configuration["JwtSettings:JWTIssuer"],
            Audience = _configuration["JwtSettings:JWTAudience"],
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}