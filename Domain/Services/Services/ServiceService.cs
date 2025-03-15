using Domain.DTO.Paging;
using Domain.DTO.Service;
using Domain.DTO.ServiceType;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories.IRepository;
using Domain.Repositories.Repository;
using Domain.Services.IServices;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepo _serviceRepo;
        private readonly IConfiguration _configuration;
        public ServiceService(IConfiguration configuration, IServiceRepo serviceRepo)
        {
            _configuration = configuration;
            _serviceRepo = serviceRepo;
        }
        public Task<int> AddService(ServiceCreateRequest request)
        {
            try
            {
                return _serviceRepo.AddService(request);
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
                return await _serviceRepo.DeleteService(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResponseData<Service>> GetServices(ServiceGetRequest request)
        {
            var model = new ResponseData<Service>();
            try
            {
                DataTable table = await _serviceRepo.GetServices(request);

                model.data = (from row in table.AsEnumerable()
                              select new Service
                              {
                                  Id = row.Field<Guid>("Id"),
                                  Name = row.Field<string>("Name"),
                                  Description = row.Field<string>("Description"),
                                  Price = row.Field<decimal>("Price"),
                                  Image = row.Field<string>("Image"),
                                  Status = row.Field<EntityStatus>("Status"),
                                  CreatedTime = row.Field<DateTimeOffset>("CreatedTime"),
                                  CreatedBy = row.Field<Guid?>("CreatedBy") != null ? row.Field<Guid>("CreatedBy") : Guid.Empty,
                                  ServiceTypeId = row.Field<Guid>("ServiceTypeId"),
                                  UnitId = row.Field<Guid>("UnitId"),
                                  Images = row.Field<string>("Images") != null
                                   ? JsonConvert.DeserializeObject<List<Images>>(row.Field<string>("Images"))
                                   : new List<Images>()
                              }).ToList();

                //phân trang
                model.CurrentPage = request.PageIndex;
                model.PageSize = request.PageSize;

                try
                {
                    //chuyển đổi và gán giá trị tổng số bản ghi
                    model.totalRecord = Convert.ToInt32(table.Rows[0]["TotalRows"]);
                }
                catch (Exception ex)
                {
                    // Nếu có lỗi total recod = 0
                    model.totalRecord = 0;
                }

                //tổng trang
                model.totalPage = (int)Math.Ceiling((double)model.totalRecord / request.PageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return model;
        }

        public async Task<List<ServiceTypeGroupDto>> GetAllServiceNamesGroupedByServiceType()
        {
            try
            {
                return await _serviceRepo.GetAllServiceNamesGroupedByServiceType();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error", ex);
            }
        }



        public async Task<Service> GetServiceById(Guid Id)
        {
            Service service = new();
            try
            {
                DataTable table = await _serviceRepo.GetServiceById(Id);
                service = (from row in table.AsEnumerable()
                           select new Service
                           {
                               Id = row.Field<Guid>("Id"),
                               Name = row.Field<string>("Name"),
                               Description = row.Field<string>("Description"),
                               Status = row.Field<EntityStatus>("Status"),
                               Price = row.Field<decimal>("Price"),
                               ServiceTypeId = row.Field<Guid>("ServiceTypeId"),
                               UnitId = row.Field<Guid>("UnitId"),
                               UnitName = row.Field<string?>("Unit"),
                               CreatedTime = row.IsNull("CreatedTime") ? (DateTimeOffset?)null : row.Field<DateTimeOffset>("CreatedTime"),
                               CreatedBy = row.IsNull("CreatedBy") ? Guid.Empty : row.Field<Guid>("CreatedBy"),

                               ModifiedTime = row.IsNull("ModifiedTime") ? (DateTimeOffset?)null : row.Field<DateTimeOffset>("ModifiedTime"),
                               ModifiedBy = row.IsNull("ModifiedBy") ? Guid.Empty : row.Field<Guid>("ModifiedBy"),

                               Deleted = row.Field<bool>("Deleted"),
                               DeletedBy = row.IsNull("DeletedBy") ? Guid.Empty : row.Field<Guid>("DeletedBy"),
                               DeletedTime = row.IsNull("DeletedTime") ? (DateTimeOffset?)null : row.Field<DateTimeOffset>("DeletedTime"),
                               Images = row.Field<string>("Images") != null
                                   ? JsonConvert.DeserializeObject<List<Images>>(row.Field<string>("Images"))
                                   : new List<Images>()
                           }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy dịch vụ", ex);
            }
            return service;
        }


        public Task<int> UpdateService(ServiceUpdateRequest request)
        {
            try
            {
                return _serviceRepo.UpdateService(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
