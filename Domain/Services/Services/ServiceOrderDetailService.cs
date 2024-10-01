using Domain.DTO.Paging;
using Domain.DTO.ServiceOrderDetail;
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
    public class ServiceOrderDetailService : IServiceOrderDetailService
    {
        private readonly ServiceOrderDetailRepo _serviceOrderDetailRepo;
        private readonly IConfiguration _configuration;
        public ServiceOrderDetailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _serviceOrderDetailRepo = new ServiceOrderDetailRepo(_configuration);
        }
        public Task<int> AddServiceOrderDetail(ServiceOrderDetailCreateRequest request)
        {
            try
            {
                return _serviceOrderDetailRepo.AddServiceOrderDetail(request);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<int> DeleteServiceOrderDetail(ServiceOrderDetailDeleteRequest request)
        {
            try
            {
                return await _serviceOrderDetailRepo.DeleteServiceOrderDetail(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ServiceOrderDetail> GetServiceOrderDetailById(Guid Id)
        {
            ServiceOrderDetail sv = new ServiceOrderDetail();
            try
            {
                DataTable table = await _serviceOrderDetailRepo.GetServiceOrderDetailById(Id);
                sv = (from row in table.AsEnumerable()
                      select new ServiceOrderDetail
                      {
                          Id = row.Field<Guid>("Id"),
                          ServiceOrderId = row.Field<Guid>("ServiceOrderId"),
                          ServiceId = row.Field<Guid>("ServiceId"),
                          Amount = row.Field<double>("Amount"),
                          Price = row.Field<decimal>("Price"),
                          Status = row.Field<EntityStatus>("Status"),
                          CreatedTime = row.Field<DateTimeOffset>("CreatedTime"),
                          CreatedBy = row.Field<Guid?>("CreatedBy") != null ? row.Field<Guid>("CreatedBy") : Guid.Empty,
                          ModifiedTime = row.Field<DateTimeOffset>("ModifiedTime"),
                          ModifiedBy = row.Field<Guid?>("ModifiedBy") ?? Guid.Empty,
                          Deleted = row.Field<bool>("Deleted"),
                          DeletedBy = row.Field<Guid?>("DeletedBy") ?? Guid.Empty,
                          DeletedTime = row.Field<DateTimeOffset>("DeletedTime")
                      }).FirstOrDefault();
            }
            catch (Exception exx)
            {

                throw exx;
            }
            return sv;
        }

        public async Task<ResponseData<ServiceOrderDetail>> GetServiceOrderDetailByServiceOrderId(Guid id)
        {
            var model = new ResponseData<ServiceOrderDetail>();
            try
            {
                DataTable table = await _serviceOrderDetailRepo.GetServiceOrderDetailByServiceOrderId(id);

                model.data = (from row in table.AsEnumerable()
                              select new ServiceOrderDetail
                              {
                                  Id = row.Field<Guid>("Id"),
                                  ServiceOrderId = row.Field<Guid>("ServiceOrderId"),
                                  ServiceId = row.Field<Guid>("ServiceId"),
                                  Amount = row.Field<double>("Amount"),
                                  Price = row.Field<decimal>("Price"),
                                  Status = row.Field<EntityStatus>("Status"),
                                  CreatedTime = row.Field<DateTimeOffset>("CreatedTime"),
                                  CreatedBy = row.Field<Guid?>("CreatedBy") != null ? row.Field<Guid>("CreatedBy") : Guid.Empty,
                              }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }

        public async Task<ResponseData<ServiceOrderDetail>> GetServiceOrderDetails(ServiceOrderDetailGetRequest request)
        {
            var model = new ResponseData<ServiceOrderDetail>();
            try
            {
                DataTable table = await _serviceOrderDetailRepo.GetServiceOrderDetails(request);

                model.data = (from row in table.AsEnumerable()
                              select new ServiceOrderDetail
                              {
                                  Id = row.Field<Guid>("Id"),
                                  ServiceOrderId = row.Field<Guid>("ServiceOrderId"),
                                  ServiceId = row.Field<Guid>("ServiceId"),
                                  Amount = row.Field<double>("Amount"),
                                  Price = row.Field<decimal>("Price"),
                                  Status = row.Field<EntityStatus>("Status"),
                                  CreatedTime = row.Field<DateTimeOffset>("CreatedTime"),
                                  CreatedBy = row.Field<Guid?>("CreatedBy") != null ? row.Field<Guid>("CreatedBy") : Guid.Empty,
                              }).ToList();
                //phaan trang
                model.CurrentPage = request.PageIndex;
                model.PageSize = request.PageSize;

                try
                {
                    model.totalRecord = Convert.ToInt32(table.Rows[0]["TotalRows"]);
                }
                catch (Exception ex)
                {
                    model.totalRecord = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }

        public Task<int> UpdateServiceOrderDetail(ServiceOrderDetailUpdateRequest request)
        {
            try
            {
                return _serviceOrderDetailRepo.UpdateServiceOrderDetail(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
