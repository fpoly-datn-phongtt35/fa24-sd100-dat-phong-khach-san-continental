using Domain.DTO.Floor;
using Domain.Models;
using Domain.Repositories.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.StoredProcedure;

namespace Domain.Repositories.Repository
{
    public class FloorRepo : IFloorRepo
    {
        private readonly BuildingRepo _floorRepo;
        private static DbWorker _DbWorker;
        private readonly IConfiguration _configuration;
        public FloorRepo(IConfiguration configuration)
        {
            _configuration = configuration;
            _DbWorker = new DbWorker(StoredProcedureConstant.Continetal);
        }
        public async Task<int> AddFloor(FloorCreateRequest request)
        {
            try
            {
                var building = await _floorRepo.GetBuildingById(request.BuildingId);
                if (building is null)
                {
                    throw new ArgumentException("Building not found");
                }

                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@BuildingId", request.BuildingId),
                    new SqlParameter("@Name", !string.IsNullOrEmpty(request.Name) ? request.Name : DBNull.Value),
                    new SqlParameter("@NumberOfRoom", request.NumberOfRoom),
                    new SqlParameter("@Status", (int)request.Status),
                    new SqlParameter("@CreatedTime", request.CreatedTime),
                    new SqlParameter("@CreatedBy", request.CreatedBy != null ? request.CreatedBy : DBNull.Value)
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_InsertFloor, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    

        public async Task<int> DeleteFloor(FloorDeleteRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", request.Id != null ? (object)request.Id : DBNull.Value),
                    new SqlParameter("@DeletedTime", DateTime.Now),
                    new SqlParameter("@DeletedBy", request.DeletedBy != Guid.Empty ? (object)request.DeletedBy : DBNull.Value)
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_DeleteFloor, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> GetFloor(FloorGetRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Name", !string.IsNullOrEmpty(request.Name) ? request.Name : DBNull.Value),
                    new SqlParameter("@PageSize", request.PageSize),
                    new SqlParameter("@PageIndex", request.PageIndex)
                };

                return _DbWorker.GetDataTable(StoredProcedureConstant.SP_GetListFloor, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> GetFloorById(Guid id)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", id != null ? id : DBNull.Value ),
                };

                return _DbWorker.GetDataTable(StoredProcedureConstant.SP_GetFloorById, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> GetFloorByBuildingId(FloorGetRequest request, Guid buildingId)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
            new SqlParameter("@BuildingId", buildingId),
            new SqlParameter("@PageSize", request.PageSize),
            new SqlParameter("@PageIndex", request.PageIndex)
                };

                return _DbWorker.GetDataTable(StoredProcedureConstant.SP_GetFloorByBuildingId, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> UpdateFloor(FloorUpdateRequest request)
        {
            var building = await _floorRepo.GetBuildingById(request.BuildingId);
            if (building is null)
            {
                throw new ArgumentException("Building not found");
            }
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", request.Id != null ? request.Id : DBNull.Value),
                    new SqlParameter("@BuildingId", request.BuildingId),
                    new SqlParameter("@Name", !string.IsNullOrEmpty(request.Name) ? request.Name : DBNull.Value),
                    new SqlParameter("@NumberOfRoom", request.NumberOfRoom),
                    new SqlParameter("@Status",1),
                    new SqlParameter("@ModifiedTime",DateTime.Now),
                    new SqlParameter("@ModifiedBy", request.ModifiedBy!= null ? request.ModifiedBy : DBNull.Value)
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_UpdateBuilding, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
