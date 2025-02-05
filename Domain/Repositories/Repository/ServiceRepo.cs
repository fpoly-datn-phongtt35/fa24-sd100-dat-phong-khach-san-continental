﻿using Domain.DTO.Service;
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
    public class ServiceRepo : IServiceRepo
    {
        private readonly ServiceTypeRepo _serviceTypeRepo;
        private readonly DbWorker _DbWorker;
        private readonly IConfiguration _configuration;
        public ServiceRepo(IConfiguration configuration)
        {
            _configuration = configuration;
            _DbWorker = new DbWorker(StoredProcedureConstant.Continetal);
            _serviceTypeRepo = new ServiceTypeRepo(_configuration);
        }
        public async Task<int> AddService(ServiceCreateRequest request)
        {
            try
            {
                var serviceType = await _serviceTypeRepo.GetServiceTypeById(request.ServiceTypeId);
                if (serviceType is null)
                {
                    throw new ArgumentException("ServiceType not found");
                }

                SqlParameter[] sqlParameters = new SqlParameter[]
                {                   
                    new SqlParameter("@ServiceTypeId", request.ServiceTypeId),
                    new SqlParameter("@Name", !string.IsNullOrEmpty(request.Name) ? request.Name : DBNull.Value),
                    new SqlParameter("@Description", !string.IsNullOrEmpty(request.Description) ? request.Description : DBNull.Value),
                    new SqlParameter("@Price", request.Price),
                    new SqlParameter("@Image", request.Image != null ? request.Image : DBNull.Value),
                    new SqlParameter("@Unit", request.Unit),
                    new SqlParameter("@Status", (int)request.Status),
                    new SqlParameter("@CreatedTime", request.CreatedTime),
                    new SqlParameter("@CreatedBy", request.CreatedBy != null ? request.CreatedBy : DBNull.Value)
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_InsertService, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeleteService(ServiceDeleteRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", request.Id != null ? (object)request.Id : DBNull.Value),
                    new SqlParameter("@DeletedTime", DateTime.Now),
                    new SqlParameter("@DeletedBy", request.DeletedBy != Guid.Empty ? (object)request.DeletedBy : DBNull.Value)
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_DeleteService, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> GetServices(ServiceGetRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@PageSize", request.PageSize),
                    new SqlParameter("@PageIndex", request.PageIndex),
                    new SqlParameter("@Name", request.Name),
                    new SqlParameter("@ServiceTypeId", request.ServiceTypeId),
                    new SqlParameter("@MinPrice", request.MinPrice != 0 ? request.MinPrice : DBNull.Value),
                    new SqlParameter("@MaxPrice", request.MaxPrice != 0 ? request.MaxPrice : DBNull.Value),
                    new SqlParameter("@Status", request.Status)
                };

                return _DbWorker.GetDataTable(StoredProcedureConstant.SP_GetListService, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ServiceTypeGroupDto>> GetAllServiceNamesGroupedByServiceType()
        {
            try
            {
                var dataTable = await _DbWorker.GetDataTableAsync(
                    StoredProcedureConstant.SP_GetAllServiceNamesGroupedByServiceType,
                    Array.Empty<SqlParameter>() 
                );

                var groupedData = dataTable.AsEnumerable()
                    .GroupBy(row => row.Field<string>("ServiceTypeName"))
                    .Select(group => new ServiceTypeGroupDto
                    {
                        ServiceTypeName = group.Key, 
                        ServiceIds = group.Select(row => row.Field<Guid>("ServiceId")).ToList(),
                        ServiceNames = group.Select(row => row.Field<string>("ServiceName")).ToList() 
                    })
                    .ToList();

                return groupedData;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error", ex);
            }
        }


        public async Task<DataTable> GetServiceById(Guid id)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", id != null ? id : DBNull.Value ),
                };

                return _DbWorker.GetDataTable(StoredProcedureConstant.SP_GetListService, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<int> UpdateService(ServiceUpdateRequest request)
        {
            var serviceType = await _serviceTypeRepo.GetServiceTypeById(request.ServiceTypeId);
            if (serviceType is null)
            {
                throw new ArgumentException("ServiceType not found");
            }
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", request.Id != null ? request.Id : DBNull.Value),
                    new SqlParameter("@ServiceTypeId", request.ServiceTypeId),
                    new SqlParameter("@Name",!string.IsNullOrEmpty(request.Name) ? request.Name : DBNull.Value),
                    new SqlParameter("@Description",!string.IsNullOrEmpty(request.Description) ? request.Description : DBNull.Value),
                    new SqlParameter("@Price",request.Price),
                    new SqlParameter("@Image", request.Image),
                    new SqlParameter("@Unit",request.Unit),
                    new SqlParameter("@Status",request.Status),
                    new SqlParameter("@Deleted",request.Deleted),
                    new SqlParameter("@ModifiedTime",DateTime.Now),
                    new SqlParameter("@ModifiedBy", request.ModifiedBy!= null ? request.ModifiedBy : DBNull.Value)
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_UpdateService, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
