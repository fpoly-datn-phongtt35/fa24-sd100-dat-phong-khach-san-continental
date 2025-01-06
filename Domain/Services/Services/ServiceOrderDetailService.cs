
using Domain.DTO.Paging;
using Domain.DTO.ServiceOrderDetail;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories.IRepository;
using Domain.Repositories.Repository;
using Domain.Services.IServices;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

        public async Task<List<ServiceOrderDetailResponse>> GetListServiceOrderDetailByRoomBookingDetailId(Guid id)
        {
            try
            {
                DataTable dt = await _serviceOrderDetailRepo.GetListServiceOrderDetailByRoomBookingDetailId(id);
                var sv = (from row in dt.AsEnumerable()
                          select new ServiceOrderDetailResponse
                          {
                              Id = row.Field<Guid>("Id"),
                              ServiceId = row.Field<Guid>("ServiceId"),
                              Amount = row.Field<double>("Amount"),
                              Name = row.Field<string?>("Name"),
                              Unit = row.Field<UnitType>("Unit").ToString(),
                              Price = row.Field<decimal>("Price"),
                              Quantity = row.Field<int>("Quantity"),
                              Description = row.Field<string?>("Description"),
                              ExtraPrice = row.Field<decimal>("ExtraPrice"),
                              Status = row.Field<EntityStatus>("Status"),
                              StatusName = row.Field<EntityStatus>("Status").ToString(),
                              CreatedTime = row.Field<DateTimeOffset?>("CreatedTime"),
                              CreatedBy = row.Field<Guid?>("CreatedBy") != null ? row.Field<Guid>("CreatedBy") : Guid.Empty,
                              ModifiedTime = row.Field<DateTimeOffset?>("ModifiedTime"),
                              ModifiedBy = row.Field<Guid?>("ModifiedBy") ?? Guid.Empty,
                              Deleted = row.Field<bool>("Deleted"),
                              DeletedBy = row.Field<Guid?>("DeletedBy") ?? Guid.Empty,
                              DeletedTime = row.Field<DateTimeOffset?>("DeletedTime")
                          }).ToList();
                return sv;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<int> UpsertServiceOrderDetail(ServiceOrderDetail request)
        {
            try
            {
                return _serviceOrderDetailRepo.UpsertServiceOrderDetail(request);
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
                          ServiceId = row.Field<Guid>("ServiceId"),
                          Amount = row.Field<double>("Amount"),
                          Price = row.Field<decimal>("Price"),
                          ExtraPrice = row.Field<decimal>("ExtraPrice"),
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

        public async Task<ResponseData<ServiceOrderDetail>> GetServiceOrderDetailByRoomBookingDetailId(Guid id)
        {
            var model = new ResponseData<ServiceOrderDetail>();
            try
            {
                DataTable table = await _serviceOrderDetailRepo.GetServiceOrderDetailByRoomBookingDetailId(id);

                model.data = (from row in table.AsEnumerable()
                              select new ServiceOrderDetail
                              {
                                  Id = row.Field<Guid>("Id"),
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
                                  RoomBookingDetailId = row.Field<Guid>("RoomBookingDetailId"),
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

                model.totalPage = (int)Math.Ceiling((double)model.totalRecord / request.PageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }
    }
}
