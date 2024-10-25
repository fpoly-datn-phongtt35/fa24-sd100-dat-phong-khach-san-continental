using Domain.DTO.Paging;
using Domain.DTO.ServiceOrder;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories.IRepository;
using Domain.Repositories.Repository;
using Domain.Services.IServices;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Domain.Services.Services
{
    public class ServiceOrderService : IServiceOrderService
    {
        private readonly IServiceOrderRepo _serviceOrderRepo;
        private readonly IConfiguration _configuration;

        public ServiceOrderService(IConfiguration configuration)
        {
            _configuration = configuration;
            _serviceOrderRepo = new ServiceOrderRepo(_configuration);
        }

        public Task<Guid> AddServiceOrder(ServiceOrderCreateRequest request)
        {
            try
            {
                return _serviceOrderRepo.AddServiceOrder(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<int> DeleteServiceOrder(ServiceOrderDeleteRequest request)
        {
            try
            {
                return _serviceOrderRepo.DeleteServiceOrder(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public async Task<ServiceOrder> GetServiceOrderById(Guid Id)
        {
            ServiceOrder serviceOrder = new();
            try
            {
                DataTable table = await _serviceOrderRepo.GetServiceOrderById(Id);
                serviceOrder = (from row in table.AsEnumerable()
                               select new ServiceOrder
                               {
                                   Id = row.Field<Guid>("Id"),
                                   RoomBookingId = row.IsNull("RoomBookingId") ? (Guid?)null : row.Field<Guid>("RoomBookingId"),
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
            return serviceOrder;
        }

        public async Task<ResponseData<ServiceOrder>> GetServiceOrders(ServiceOrderGetRequest request)
        {
            var model = new ResponseData<ServiceOrder>();
            try
            {
                DataTable table = await _serviceOrderRepo.GetServiceOrders(request);
                model.data = (from row in table.AsEnumerable()
                              select new ServiceOrder
                              {
                                  Id = row.Field<Guid>("Id"),
                                  //xử lý nếu RoomBookingDetailId là null
                                  RoomBookingId = row.IsNull("RoomBookingId") ? (Guid?)null : row.Field<Guid>("RoomBookingId"),
                                  Status = row.Field<EntityStatus>("Status"),
                                  CreatedTime = row.Field<DateTimeOffset>("CreatedTime"),
                                  CreatedBy = row.Field<Guid?>("CreatedBy") != null ? row.Field<Guid>("CreatedBy") : Guid.Empty,
                                  ModifiedTime = row.Field<DateTimeOffset>("ModifiedTime"),
                                  ModifiedBy = row.Field<Guid?>("ModifiedBy") != null ? row.Field<Guid>("ModifiedBy") : Guid.Empty,
                                  Deleted = row.Field<bool>("Deleted"),
                                  DeletedBy = row.Field<Guid?>("DeletedBy") != null ? row.Field<Guid>("DeletedBy") : Guid.Empty,
                                  DeletedTime = row.Field<DateTimeOffset>("DeletedTime")
                              }).ToList();
                model.CurrentPage = request.PageIndex;
                model.PageSize = request.PageSize;
                try
                {
                    // chuyển và gnas giá trị
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

        public Task<int> UpdateServiceOrder(ServiceOrderUpdateRequest request)
        {
            try
            {
                return _serviceOrderRepo.UpdateServiceOrder(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
