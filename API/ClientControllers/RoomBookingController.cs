﻿using Domain.DTO.Customer;
using Domain.DTO.Paging;
using Domain.DTO.PaymentHistory;
using Domain.DTO.RoomBooking;
using Domain.Enums;
using Domain.Repositories.IRepository;
using Domain.Repositories.Repository;
using Domain.Services.IServices;
using Domain.Services.IServices.IRoomBooking;
using Domain.Services.Services;
using Domain.Services.Services.RoomBooking;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using System.Data;

namespace API.ClientControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomBookingController : ControllerBase
    {
        private readonly IRoomBookingCreateForCustomerService _roomBookingCreateService;
        private readonly IRoomBookingGetService _getListRoomBookingsService;
        private readonly IRoomBookingDetailRepository _rbdRepository;
        private readonly PayOS _payOS;
        private readonly IPaymentHistoryService _paymentHistoryService;
        private readonly IRoomBookingUpdateService _roomBookingUpdateService;
        public RoomBookingController(IRoomBookingCreateForCustomerService roomBookingCreateService, 
            IRoomBookingGetService getListRoomBookingsService,
            IPaymentHistoryService paymentHistoryService,
            PayOS payOS,
            IRoomBookingUpdateService roomBookingUpdateService,
            IRoomBookingDetailRepository rbdRepository)
        {
            _roomBookingCreateService = roomBookingCreateService;
            _getListRoomBookingsService = getListRoomBookingsService;
            _paymentHistoryService = paymentHistoryService;
            _payOS = payOS;
            _roomBookingUpdateService = roomBookingUpdateService;
            _rbdRepository = rbdRepository;
        }
        [HttpPost("CreateRoomBookingForCustomer")]
        public async Task<ActionResult<Guid>> CreateRoomBookingForCustomer([FromBody]RoomBookingCreateRequestForCustomer request)
        {
            try
            {
                Guid newId = await _roomBookingCreateService.CreateRoomBookingForCustomer(request);

                if (newId == Guid.Empty)
                {
                    return BadRequest("Unable to create room booking.");
                }

                return Ok(newId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost("GetListRoomBookingByCustomerId")]
        public async Task<ResponseData<RoomBookingResponseForCustomer>> GetListRoomBookingByCustomerId
        (RoomBookingGetRequestByCustomer request)
        {
            try
            {
                 UpdatePaymentHistory();
                return await _getListRoomBookingsService.GetListRoomBookingByCustomerId(request);
            }
            catch (Exception e)
            {
                throw new NullReferenceException("The list of room bookings could not be retrieved", e);
            }
        }
        private async Task UpdatePaymentHistory()
        {
            // Tạo request để lấy danh sách các bản ghi cần xử lý
            var request = new PaymentHistoryGetRequest
            {
                Amount = 0
            };

            // Lấy danh sách các bản ghi có Amount = 0
            var paymentHistories = await _paymentHistoryService.GetListPaymentHistory(request);

            // Tạo danh sách các tác vụ xử lý song song
            var tasks = paymentHistories.data.Select(async paymentHistory =>
            {
                try
                {
                    if (paymentHistory.OrderCode == 0)
                    {
                        // Xóa bản ghi nếu OrderCode = 0
                        await _paymentHistoryService.DeletePaymentHistory(paymentHistory.Id);
                    }
                    else
                    {
                        // Gọi API để lấy thông tin trạng thái
                        var paymentInfo = await _payOS.getPaymentLinkInformation(paymentHistory.OrderCode);
                        if (paymentInfo.status == "PAID")
                        {
                            // Lấy thông tin RoomBooking
                            var roomBooking = await _getListRoomBookingsService.GetRoomBookingById(paymentHistory.RoomBookingId);
                            if (roomBooking == null) return;

                            // Cập nhật Amount dựa trên Note
                            if (paymentHistory.Note == PaymentType.Bill)
                            {
                                await _paymentHistoryService.UpdatePaymentHistoryAmount(paymentHistory.Id, paymentInfo.amount);
                            }
                            else if (paymentHistory.Note == PaymentType.Deposit)
                            {
                                await _paymentHistoryService.UpdatePaymentHistoryAmount(paymentHistory.Id,
                                    (int)roomBooking.TotalRoomPrice * 20 / 100);
                                await _roomBookingUpdateService.UpdateRoomBookingStatus(paymentHistory.RoomBookingId, 5);
                            }
                        }
                        else if (paymentInfo.status == "CANCELLED" && paymentHistory.Note == PaymentType.Bill)
                        {
                            // Xóa bản ghi nếu trạng thái là CANCELLED và loại là Bill
                            await _paymentHistoryService.DeletePaymentHistory(paymentHistory.Id);
                        }
                        else if (paymentInfo.status == "CANCELLED" && paymentHistory.Note == PaymentType.Deposit)
                        {
                            // Xóa bản ghi nếu trạng thái là CANCELLED và loại là Deposit
                            await _paymentHistoryService.DeletePaymentHistory(paymentHistory.Id);
                            await _roomBookingUpdateService.UpdateRoomBookingStatus(paymentHistory.RoomBookingId, 3);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            });

            // Chờ tất cả các tác vụ xử lý song song hoàn tất
            await Task.WhenAll(tasks);
        }
    }
}
