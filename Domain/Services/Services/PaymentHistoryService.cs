using Domain.DTO.Paging;
using Domain.DTO.PaymentHistory;
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
    public class PaymentHistoryService : IPaymentHistoryService
    {
        private readonly IPaymentHistoryRepository _paymentHistoryRepo;
        private readonly IConfiguration _configuration;

        public PaymentHistoryService(IPaymentHistoryRepository paymentHistoryRepo, IConfiguration configuration)
        {
            _paymentHistoryRepo = paymentHistoryRepo;
            _configuration = configuration;
        }

        public async Task<int> AddPaymentHistory(PaymentHistoryCreateRequest request)
        {
            try
            {
                return await _paymentHistoryRepo.AddPaymentHistory(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeletePaymentHistory(Guid id)
        {
            try
            {
                return await _paymentHistoryRepo.DeletePaymentHistory(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<ResponseData<PaymentHistory>> GetListPaymentHistory(PaymentHistoryGetRequest request)
        {
            var model = new ResponseData<PaymentHistory>();
            try
            {
                DataTable dataTable = await _paymentHistoryRepo.GetListPaymentHistory(request);
                model.data = (from row in dataTable.AsEnumerable()
                              select new PaymentHistory
                              {
                                  Id = row.Field<Guid>("Id"),
                                  OrderCode = row.Field<int>("OrderCode"),
                                  RoomBookingId = row.Field<Guid>("RoomBookingId"),
                                  PaymentMethod = row.Field<PaymentMethod>("PaymentMethod"),
                                  Amount = row.Field<decimal>("Amount"),
                                  PaymentTime = row.Field<DateTimeOffset>("PaymentTime"),
                                  Note = row.Field<PaymentType>("Note")
                              }).ToList();
                model.CurrentPage = request.PageIndex;
                model.PageSize = request.PageSize;
                try
                {
                    // Thử chuyển đổi và gán giá trị
                    model.totalRecord = Convert.ToInt32(dataTable.Rows[0]["TotalRows"]);
                }
                catch (Exception ex)
                {
                    // Nếu có lỗi xảy ra (ví dụ: không tìm thấy cột, không thể chuyển đổi), gán giá trị mặc định là 0
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
        public async Task<PaymentHistory> GetPaymentHistoryById(Guid id)
        {
            PaymentHistory ph = new();
            try
            {
                DataTable table = await _paymentHistoryRepo.GetPaymentHistoryById(id);
                ph = (from row in table.AsEnumerable()
                           select new PaymentHistory
                           {
                               Id = row.Field<Guid>("Id"),
                               OrderCode = row.Field<int>("OrderCode"),
                               RoomBookingId = row.Field<Guid>("RoomBookingId"),
                               PaymentMethod = row.Field<PaymentMethod>("PaymentMethod"),
                               Amount = row.Field<decimal>("Amount"),
                               PaymentTime = row.Field<DateTimeOffset>("PaymentTime"),
                               Note = row.Field<PaymentType>("Note")
                           }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ph;
        }
        
        public async Task<PaymentHistory> GetPaymentHistoryByOrderCode(long orderCode)
        {
            PaymentHistory paymentHistory = new();
            try
            {   
                DataTable table = await _paymentHistoryRepo.GetPaymentHistoryByOrderCode(orderCode);
                paymentHistory = (from row in table.AsEnumerable()
                    select new PaymentHistory
                    {
                        Id = row.Field<Guid>("Id"),
                        OrderCode = row.Field<int>("OrderCode"),
                        RoomBookingId = row.Field<Guid>("RoomBookingId"),
                        PaymentMethod = row.Field<PaymentMethod>("PaymentMethod"),
                        Amount = row.Field<decimal>("Amount"),
                        PaymentTime = row.Field<DateTimeOffset>("PaymentTime"),
                        Note = row.Field<PaymentType>("Note")
                    }).FirstOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return paymentHistory;
        }

        public async Task<decimal> GetTotalPaidAmountByRoomBookingId(Guid roomBookingId)
        {
            return await _paymentHistoryRepo.GetTotalPaidAmountByRoomBookingId(roomBookingId);
        }

        public Task<int> UpdatePaymentHistoryAmount(Guid id, decimal amount)
        {
            try
            {
                return _paymentHistoryRepo.UpdatePaymentHistoryAmount(id, amount);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
