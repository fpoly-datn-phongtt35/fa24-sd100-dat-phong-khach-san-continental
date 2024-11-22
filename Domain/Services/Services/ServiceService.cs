using Domain.DTO.Paging;
using Domain.DTO.Service;
using Domain.DTO.ServiceType;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories.IRepository;
using Domain.Repositories.Repository;
using Domain.Services.IServices;
using Microsoft.Extensions.Configuration;
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
                                  Unit = (UnitType)row.Field<int>("Unit"),
                                  Image = row.Field<string>("Image"),
                                  Status = row.Field<EntityStatus>("Status"),
                                  CreatedTime = row.Field<DateTimeOffset>("CreatedTime"),
                                  CreatedBy = row.Field<Guid?>("CreatedBy") != null ? row.Field<Guid>("CreatedBy") : Guid.Empty,
                                  ServiceTypeId = row.Field<Guid>("ServiceTypeId"),
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
                                   Image = row.Field<string>("Image"),
                                   Unit = (UnitType)row.Field<int>("Unit"),
                                   ServiceTypeId = row.Field<Guid>("ServiceTypeId"),
                                   CreatedTime = row.Field<DateTimeOffset>("CreatedTime"),
                                   CreatedBy = row.Field<Guid?>("CreatedBy") != null ? row.Field<Guid>("CreatedBy") : Guid.Empty
                                   ,
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
            return service;
        }

        //public async Task<ResponseData<Service>> GetServiceByTypeId(ServiceGetRequest request, Guid serviceTypeId)
        //{
        //    var model = new ResponseData<Service>();
        //    try
        //    {
        //        // Gọi repo để lấy DataTable
        //        DataTable table = await _serviceRepo.GetServiceByTypeId(request, serviceTypeId);

        //        model.data = (from row in table.AsEnumerable()
        //                      select new Service
        //                      {
        //                          Id = row.Field<Guid>("Id"),
        //                          Name = row.Field<string>("Name"),
        //                          Description = row.Field<string>("Description"),
        //                          Status = row.Field<EntityStatus>("Status"),
        //                          Price = row.Field<decimal>("Price"),
        //                          Unit = (UnitType)row.Field<int>("Unit"),
        //                          CreatedTime = row.Field<DateTimeOffset>("CreatedTime"),
        //                          CreatedBy = row.Field<Guid?>("CreatedBy") ?? Guid.Empty,
        //                          ModifiedTime = row.Field<DateTimeOffset>("ModifiedTime"),
        //                          ModifiedBy = row.Field<Guid?>("ModifiedBy") ?? Guid.Empty,
        //                          Deleted = row.Field<bool>("Deleted"),
        //                          DeletedBy = row.Field<Guid?>("DeletedBy") ?? Guid.Empty,
        //                          DeletedTime = row.Field<DateTimeOffset>("DeletedTime")
        //                      }).ToList();

        //        // Phân trang
        //        model.CurrentPage = request.PageIndex;
        //        model.PageSize = request.PageSize;

        //        try
        //        {
        //            // Gán giá trị tổng số bản ghi
        //            model.totalRecord = table.AsEnumerable().FirstOrDefault()?.Field<int>("TotalRows") ?? 0;
        //        }
        //        catch (Exception ex)
        //        {
        //            // Nếu có lỗi, gán totalRecord = 0
        //            model.totalRecord = 0;
        //        }

        //        // Tổng số trang
        //        model.totalPage = (int)Math.Ceiling((double)model.totalRecord / request.PageSize);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return model;
        //}




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
