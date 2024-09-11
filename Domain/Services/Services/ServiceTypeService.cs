using Domain.DTO.Paging;
using Domain.DTO.ServiceType;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories.Repository;
using Domain.Services.IServices;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.StoredProcedure;

namespace Domain.Services.Services
{
    public class ServiceTypeService : IServiceTypeService
    {
        private readonly ServiceTypeRepo _serviceTypeRepo;
        private readonly IConfiguration _configuration;
        public ServiceTypeService(IConfiguration configuration)
        {
            _configuration = configuration;
            _serviceTypeRepo = new ServiceTypeRepo(_configuration);
        }
        public Task<int> AddServiceType(ServiceTypeCreateRequest request)
        {
            try
            {
               return _serviceTypeRepo.AddServiceType(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<int> DeleteServiceType(ServiceTypeDeleteRequest request)
        {
            try
            {
                return _serviceTypeRepo.DeleteServiceType(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ServiceType> GetServiceTypeById(Guid Id)
        {
            ServiceType serviceType = new();
            ServiceTypeGetByIdRequest request = new ServiceTypeGetByIdRequest()
            {
                Id = Id
            };
            try 
            {
                DataTable table = await _serviceTypeRepo.GetServiceTypeById(request);
                serviceType = (from row in table.AsEnumerable()
                               select new ServiceType
                               {
                                   Id = row.Field<Guid>("Id"),
                                   Name = row.Field<string>("Name"),
                                   Description = row.Field<string>("Description"),
                                   Status = row.Field<EntityStatus>("Status"),
                                   CreatedTime = row.Field<DateTimeOffset>("CreatedTime"),
                                   CreatedBy = row.Field<Guid?>("CreatedBy") != null ? row.Field<Guid>("CreatedBy") : Guid.Empty,
                                   ModifiedTime = row.Field<DateTimeOffset>("ModifiedTime"),
                                   ModifiedBy = row.Field<Guid?>("ModifiedBy") != null ? row.Field<Guid>("ModifiedBy") : Guid.Empty,
                                   Deleted = row.Field<bool>("Deleted"),
                                   DeletedBy = row.Field<Guid?>("DeletedBy") != null ? row.Field<Guid>("DeletedBy") : Guid.Empty,
                                   DeletedTime = row.Field<DateTimeOffset>("DeletedTime")
                               }).FirstOrDefault();
            }
            catch (Exception ex) 
            {
                throw ex;
            }
            return serviceType;
        }

        public async Task<ResponseData<ServiceType>> GetServiceTypes(ServiceTypeGetRequest Search)
        {
            var model = new ResponseData<ServiceType>();
            try
            {
                DataTable table = await _serviceTypeRepo.GetServiceTypes(Search);
                model.data = (from row in table.AsEnumerable()
                                  select new ServiceType
                                  {
                                      Id = row.Field<Guid>("Id"),
                                      Name = row.Field<string>("Name"),
                                      Description = row.Field<string>("Description"),
                                      Status = row.Field<EntityStatus>("Status"),
                                      CreatedTime = row.Field<DateTimeOffset>("CreatedTime"),
                                      CreatedBy = row.Field<Guid?>("CreatedBy") != null ? row.Field<Guid>("CreatedBy") : Guid.Empty,
                                      ModifiedTime = row.Field<DateTimeOffset>("ModifiedTime"),
                                      ModifiedBy = row.Field<Guid?>("ModifiedBy") != null ? row.Field<Guid>("ModifiedBy") : Guid.Empty,
                                      Deleted = row.Field<bool>("Deleted"),
                                      DeletedBy = row.Field<Guid?>("DeletedBy") != null ? row.Field<Guid>("DeletedBy") : Guid.Empty,
                                      DeletedTime = row.Field<DateTimeOffset>("DeletedTime")
                                  }).ToList();
                model.CurrentPage = Search.PageIndex;
                model.PageSize = Search.PageSize;
                try
                {
                    // Thử chuyển đổi và gán giá trị
                    model.totalRecord = Convert.ToInt32(table.Rows[0]["TotalRows"]);
                }
                catch (Exception ex)
                {
                    // Nếu có lỗi xảy ra (ví dụ: không tìm thấy cột, không thể chuyển đổi), gán giá trị mặc định là 0
                    model.totalRecord = 0;
                }
                model.totalPage = (int)Math.Ceiling((double)model.totalRecord / Search.PageSize);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }

        public Task<int> UpdateServiceType(ServiceTypeUpdateRequest request)
        {
            try
            {
                return _serviceTypeRepo.UpdateServiceType(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
