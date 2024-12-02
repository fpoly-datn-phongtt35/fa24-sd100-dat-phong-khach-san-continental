using Domain.DTO.Paging;
using Domain.DTO.RoomBooking;
using Domain.DTO.RoomBookingDetail;
using Domain.Enums;
using Domain.Services.IServices.IRoomBooking;
using Domain.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Net.payOS;
using Org.BouncyCastle.Asn1.Ocsp;
using Domain.Models;
using System.Text;
using System.Security.Cryptography;
using Domain.Services.IServices;
using Domain.DTO.PaymentHistory;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomBookingController : Controller
{
    private readonly IRoomBookingGetService _roomBookingGetService;
    private readonly IRoomBookingUpdateService _roomBookingUpdateService;
    private readonly IRoomBookingCreateService _roomBookingCreateService;
    private readonly IPaymentHistoryService _paymentHistoryService;
    private readonly PayOS _payOS;

    public RoomBookingController(IRoomBookingGetService roomBookingGetService, 
        IRoomBookingUpdateService roomBookingUpdateService,
         IRoomBookingCreateService roomBookingCreateService,
         IPaymentHistoryService paymentHistoryService,
         PayOS payOS)
    {
        _roomBookingGetService = roomBookingGetService;
        _roomBookingUpdateService = roomBookingUpdateService;
        _roomBookingCreateService = roomBookingCreateService;
        _paymentHistoryService = paymentHistoryService;
        _payOS = payOS;
    }
    [HttpPost("CreateRoomBooking")]
    public async Task<int> CreateRoomBooking(RoomBookingCreateRequest request)
    {
        try
        {
            return await _roomBookingCreateService.CreateRoomBooking(request);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [HttpPost(nameof(GetFilteredRoomBookings))]
    public async Task<ResponseData<RoomBookingResponse>> GetFilteredRoomBookings
        (RoomBookingGetRequest roomBookingGetRequest)
    {
        try
        {
            return await _roomBookingGetService.GetFilteredRoomBooking(roomBookingGetRequest);
        }
        catch (Exception e)
        {
            throw new NullReferenceException("The list of room bookings could not be retrieved", e);
        }
    }
    
    [HttpPost(nameof(GetRoomBookingById))]
    public async Task<RoomBookingResponse?> GetRoomBookingById(Guid roomBookingId)
    {
        try
        {
            return await _roomBookingGetService.GetRoomBookingById(roomBookingId);
        }
        catch (Exception e)
        {
            throw new NullReferenceException("Not found the room booking", e);
        }
    }

    [HttpPut(nameof(UpdateRoomBooking))]
    public async Task<RoomBookingResponse?> UpdateRoomBooking(RoomBookingUpdateRequest roomBookingUpdateRequest)
    {
        try
        {
            return await _roomBookingUpdateService.UpdateRoomBookingAsync(roomBookingUpdateRequest);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPut(nameof(UpdateRoomBookingStatus))]
    public async Task<int> UpdateRoomBookingStatus(Guid id, int status)
    {
        try
        {
            return await _roomBookingUpdateService.UpdateRoomBookingStatus(id, status);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost("update-roombookings-status")]
    public async Task<IActionResult> UpdateRoomBookingsStatus()
    {
        try
        {
            RoomBookingGetRequest roomBookingGetRequest = new RoomBookingGetRequest
            {
                StaffId = null,
                Status = null,
                BookingType = null,
                SearchString = null
            };

            // Lấy danh sách tất cả các RoomBookingId từ cơ sở dữ liệu
            var allRoomBookings = await _roomBookingGetService.GetFilteredRoomBooking(roomBookingGetRequest);

            // Duyệt qua từng RoomBookingId
            foreach (var roomBooking in allRoomBookings.data)
            {
                try
                {
                    int orderCode = await GenerateOrderCode(roomBooking.Id);

                    //await Task.Delay(200);

                    PaymentLinkInformation paymentLinkInformation = await _payOS.getPaymentLinkInformation(orderCode);

                    if (paymentLinkInformation.status == "PAID")
                    {
                        await _roomBookingUpdateService.UpdateRoomBookingStatus(roomBooking.Id, 2);
                        // thêm 1 bản ghi vào PaymentHistory
                        await _paymentHistoryService.AddPaymentHistory(new PaymentHistoryCreateRequest
                        {
                            RoomBookingId = roomBooking.Id,
                            PaymentMethod = PaymentMethod.BankTransfer,
                            Amount = roomBooking.TotalPrice ?? 0,
                            PaymentTime = Convert.ToDateTime(paymentLinkInformation.transactions[0].transactionDateTime),
                            Note = "Thanh toán toàn bộ"
                        });
                    }
                    else if (paymentLinkInformation.status == "CANCELLED")
                    {
                        await _roomBookingUpdateService.UpdateRoomBookingStatus(roomBooking.Id, 3);
                    }
                    else if (paymentLinkInformation.status == "PENDING")
                    {
                        await _roomBookingUpdateService.UpdateRoomBookingStatus(roomBooking.Id, 1);
                    }
                    else if (paymentLinkInformation.status == "FAILED")
                    {
                        await _roomBookingUpdateService.UpdateRoomBookingStatus(roomBooking.Id, 4);
                    }
                }
                catch (Net.payOS.Errors.PayOSError ex)
                {
                    Console.WriteLine($"Order code for RoomBookingId {roomBooking.Id} does not exist. Skipping this record.");
                    continue;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing RoomBookingId {roomBooking.Id}: {ex.Message}");
                }
            }

            return Ok(new Response(0, "Updated all room bookings' status successfully", null));
        }
        catch (Exception ex)
        {
            // Nếu có lỗi, log lỗi và trả về response thất bại
            Console.WriteLine(ex);
            return BadRequest(new Response(-1, "Failed to update room bookings' status", null));
        }
    }




    public static async Task<int> GenerateOrderCode(Guid roomBookingId)
    {
        // Chuyển GUID thành chuỗi
        string input = roomBookingId.ToString();

        // Sử dụng SHA256 để băm
        using var sha256 = SHA256.Create();
        byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

        // Lấy 4 byte đầu (32 bit) và chuyển thành int
        int orderCode = BitConverter.ToInt32(hashBytes, 0); // Dùng ToInt32 để lấy 4 byte đầu tiên

        // Đảm bảo giá trị không âm
        var result = Math.Abs(orderCode);
        return result;
    }

}