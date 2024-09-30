﻿using Domain.DTO.Building;
using Domain.DTO.Floor;
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
    public class BuildingRepo : IBuildingRepo
    {
        private static DbWorker _DbWorker;
        private readonly IConfiguration _configuration;
        public BuildingRepo(IConfiguration configuration)
        {
            _configuration = configuration;
            _DbWorker = new DbWorker(StoredProcedureConstant.Continetal);
        }
        public async Task<int> AddBuilding(BuildingCreateRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Name",!string.IsNullOrEmpty(request.Name) ? request.Name : DBNull.Value),
                    new SqlParameter("@Status",(int)request.Status),
                    new SqlParameter("@CreatedTime",DateTime.Now),
                    new SqlParameter("@CreatedBy", request.CreatedBy!= null ? request.CreatedBy : DBNull.Value)
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_InsertBuilding, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeleteBuilding(BuildingDeleteRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", request.Id != null ? (object)request.Id : DBNull.Value),
                    new SqlParameter("@DeletedTime", DateTime.Now),
                    new SqlParameter("@DeletedBy", request.DeletedBy != Guid.Empty ? (object)request.DeletedBy : DBNull.Value)
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_DeleteBuilding, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> GetBuilding(BuildingGetRequest Search)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Name", !string.IsNullOrEmpty(Search.Name) ? Search.Name : DBNull.Value),
                    new SqlParameter("@PageSize", Search.PageSize),
                    new SqlParameter("@PageIndex", Search.PageIndex)
                };

                return _DbWorker.GetDataTable(StoredProcedureConstant.SP_GetListBuilding, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> GetBuildingById(Guid Search)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", Search != null ? Search : DBNull.Value ),
                };

                return _DbWorker.GetDataTable(StoredProcedureConstant.SP_GetListBuilding, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateBuilding(BuildingUpdateRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", request.Id != null ? request.Id : DBNull.Value),
                    new SqlParameter("@Name",!string.IsNullOrEmpty(request.Name) ? request.Name : DBNull.Value),
                    new SqlParameter("@Status",request.Status),
                    new SqlParameter("@Deleted",request.Deleted),
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
