using System.Data;
using Domain.DTO.Paging;
using Domain.DTO.Unit;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Utilities.StoredProcedure;

namespace Domain.Repositories.Repository;

public class UnitRepository : IUnitRepository
{
    private readonly DbWorker worker;
    private readonly IConfiguration configuration;

    public UnitRepository(IConfiguration configuration)
    {
        worker = new DbWorker(StoredProcedureConstant.Continetal);
        this.configuration = configuration;
    }

    public async Task<Unit> addUnit(Unit unit)
    {
        try
        {
            SqlParameter[] parameters =
            {
                new("@Name", SqlDbType.NVarChar) { Value = unit.Name },
                new("@Description", SqlDbType.NVarChar) { Value = unit.Description },
                new("@Status", SqlDbType.Int) { Value = unit.Status },
                new("@NewUnitId", SqlDbType.UniqueIdentifier) { Direction = ParameterDirection.Output }
            };

            await worker.GetDataTableAsync(StoredProcedureConstant.SP_InsertUnit, parameters);
            unit.Id = (Guid)parameters[3].Value;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("An error occurred while adding the unit", e);
        }

        return unit;
    }

    public async Task<Unit?> updateUnit(Unit unit)
    {
        try
        {
            var existingUnit = getUnitById(unit.Id);
            if (existingUnit == null)
                throw new Exception("The unit you are trying to update doesn't exist");
            
            SqlParameter[] parameters = 
            {
                new("@Id", SqlDbType.UniqueIdentifier) { Value = unit.Id },
                new("@Name", SqlDbType.NVarChar) { Value = unit.Name },
                new("@Description", SqlDbType.NVarChar) { Value = unit.Description },
                new("@Status", SqlDbType.Int) { Value = unit.Status },
            };
            
            await worker.ExecuteNonQueryAsync(StoredProcedureConstant.SP_UpdateUnit, parameters);
            return await existingUnit;
        }
        catch (Exception e)
        {
            throw new Exception("An error occurred while updating the unit", e);
        }
    }

    public async Task<Unit?> deleteUnitById(Unit unit)
    {
        try
        {
            var existingUnit = getUnitById(unit.Id);
            if (existingUnit == null)
                throw new Exception("The unit you are trying to delete doesn't exist");
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new("@Id", SqlDbType.UniqueIdentifier) { Value = unit.Id },
                new("@Status", SqlDbType.Int) { Value = unit.Status },
            };
            
            await worker.ExecuteNonQueryAsync(StoredProcedureConstant.SP_DeleteUnit, parameters);
            return await existingUnit;
        }
        catch (Exception e)
        {
            throw new Exception("Some errors when deleted unit", e);
        }
    }

    public async Task<Unit?> getUnitById(Guid id)
    {
        try
        {
            SqlParameter[] parameters =
            {
                new("@Id", SqlDbType.UniqueIdentifier) { Value = id },
            };
            
            var dataTable = await worker.GetDataTableAsync(StoredProcedureConstant.SP_GetUnitById, parameters);
            if (dataTable.Rows.Count == 0)
                return null;
            
            var row = dataTable.Rows[0];
            var unit = ConvertDataRowToUnit(row);

            return unit;
        }
        catch (Exception e)
        {
            throw new ArgumentNullException("An error occurred while retrieving the unit", e);
        }
    }

    public async Task<Unit?> getUnitExists(string name)
    {
        try
        {
            SqlParameter[] parameters =
            {
                new ("@Name", SqlDbType.NVarChar) { Value = name }
            };
            
            var dataTable = await worker.GetDataTableAsync(StoredProcedureConstant.SP_GetUnitByName, parameters);
            if (dataTable.Rows.Count == 0)
                return null;
            
            var row = dataTable.Rows[0];
            var unit = ConvertDataRowToUnit(row);
            return unit;
        }
        catch (Exception e)
        {
            throw new ArgumentNullException("Error checking for duplicate unit", e);
        }
    }

    public async Task<ResponseData<UnitResponse>> getFilteredUnits(UnitGetRequest unitGetRequest)
    {
        var model = new ResponseData<UnitResponse>();
        try
        {
            SqlParameter[] parameters =
            {
                new("@PageSize", unitGetRequest.PageSize),
                new("@PageIndex", unitGetRequest.PageIndex),
                new("@SearchString", unitGetRequest.SearchString),
                new("@Status", unitGetRequest.Status)
            };
            
            var dataTable = await worker
                .GetDataTableAsync(StoredProcedureConstant.SP_GetFilteredUnits, parameters);
            var units = new List<UnitResponse>();
            foreach (DataRow row in dataTable.Rows)
            {
                var unit = ConvertDataRowToUnit(row);
                var unitResponse = unit.ToUnitResponse();
                units.Add(unitResponse);
            }
            
            model.data = units;
            model.CurrentPage = unitGetRequest.PageIndex;
            model.PageSize = unitGetRequest.PageSize;

            try
            {
                model.totalRecord = Convert.ToInt32(dataTable.Rows[0]["TotalRows"]);
            }
            catch (Exception e)
            {
                model.totalRecord = 0;
            }
            
            model.totalPage = (int)Math.Ceiling((double)model.totalRecord / unitGetRequest.PageSize);
        }
        catch (Exception e)
        {
            throw new Exception("An error occurred while retrieving filtered units", e);
        }

        return model;
    }

    private Unit ConvertDataRowToUnit(DataRow row)
    {
        return new Unit()
        {
            Id = Guid.Parse(row["Id"].ToString()!),
            Name = row["Name"].ToString()!,
            Description = row["Description"].ToString()!,
            Status = (EntityStatus)Enum.Parse(typeof(EntityStatus), row["Status"].ToString()!),
        };
    }
}