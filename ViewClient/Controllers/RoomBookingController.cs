using Domain.DTO.Room;
using Domain.DTO.RoomBooking;
using Domain.DTO.RoomBookingDetail;
using Domain.DTO.ServiceOrderDetail;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ViewClient.Repositories.IRepository;
using ViewClient.Repositories.Repository;

namespace ViewClient.Controllers
{
    public class RoomBookingController : Controller
    {
        private readonly IRoombooking _roomBookingRepo;
        private readonly IRoomBookingDetail _roomBookingDetailRepo;
        private readonly IRoom _roomRepo;
        private readonly IServiceOderDetail _serviceOderDetailRepo;
        private readonly ICustomer _customerRepo;
        public RoomBookingController(IRoombooking roomBookingRepo,
            IRoomBookingDetail roomBookingDetailRepo,
            IRoom roomRepo,
            IServiceOderDetail serviceOderDetailRepo,
            ICustomer customerRepo)
        {
            _roomBookingRepo = roomBookingRepo;
            _roomBookingDetailRepo = roomBookingDetailRepo;
            _roomRepo = roomRepo;
            _serviceOderDetailRepo = serviceOderDetailRepo;
            _customerRepo = customerRepo;
        }

        [HttpPost]
        public async Task<IActionResult> RoomBooking([FromBody] RoomBookingDetailCreateRequest roomBookingDetailCreateRequest)
        {
            try
            {
                var _UserLogin = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.UserData).Value);
                

                var room = await _roomRepo.GetRoomById(roomBookingDetailCreateRequest.RoomId);
                if (room == null)
                {
                    return NotFound();
                }
                if (_UserLogin == null)
                {

                }
                // Tạo đối tượng đặt phòng
                var roomBookingCreate = new RoomBookingCreateRequestForCustomer
                {
                    CustomerId = _UserLogin,
                    BookingType = Domain.Enums.BookingType.Online,
                    TotalPrice = room.Price,
                    TotalRoomPrice = room.Price,
                    TotalServicePrice = 0,
                    TotalExtraPrice = 0,
                    Status = Domain.Enums.RoomBookingStatus.PENDING,
                    StaffId = null,
                    CreatedBy = _UserLogin,
                    NewId = null
                };

                var roomBooking = await _roomBookingRepo.CreateRoomBooking(roomBookingCreate);

                if (roomBookingDetailCreateRequest.SelectedServices != null && roomBookingDetailCreateRequest.SelectedServices.Count > 0)
                {
                    foreach (var service in roomBookingDetailCreateRequest.SelectedServices)
                    {
                        var serviceOrderDetail = new Domain.Models.ServiceOrderDetail
                        {
                            RoomBookingId = roomBooking,
                            ServiceId = service.ServiceId,
                            Amount = Convert.ToDouble((service.Quantity) * (service.Price)),
                            Description = null,
                            Quantity = service.Quantity,
                            Price = service.Price,
                            Status = Domain.Enums.EntityStatus.Active,
                            CreatedTime = DateTime.Now,
                            CreatedBy = _UserLogin,
                            ExtraPrice = 0,
                            Deleted = false
                        };
                        await _serviceOderDetailRepo.AddServiceOrderDetail(serviceOrderDetail);
                    }
                }
                roomBookingDetailCreateRequest.RoomId = room.Id;
                roomBookingDetailCreateRequest.RoomBookingId = roomBooking;
                roomBookingDetailCreateRequest.CheckInBooking = roomBookingDetailCreateRequest.CheckInBooking;
                roomBookingDetailCreateRequest.CheckOutBooking = roomBookingDetailCreateRequest.CheckOutBooking;
                roomBookingDetailCreateRequest.CreatedBy = _UserLogin;
                roomBookingDetailCreateRequest.Status = Domain.Enums.RoomBookingStatus.PENDING;
                roomBookingDetailCreateRequest.Deposit = room.Price * 20 / 100;
                roomBookingDetailCreateRequest.Price = room.Price;
                roomBookingDetailCreateRequest.ExtraPrice = 0;
                roomBookingDetailCreateRequest.SelectedServices = null;
                var roomBookingDetail = await _roomBookingDetailRepo.CreateRoomBookingDetail(roomBookingDetailCreateRequest);

                var roomStatus = new RoomUpdateStatusRequest
                {
                    Id = room.Id,
                    Status = Domain.Enums.RoomStatus.AwaitingConfirmation,
                    ModifiedBy = _UserLogin,
                    ModifiedTime = DateTime.Now
                };
                await _roomRepo.UpdateRoomStatus(roomStatus);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return View("Details", "Room");
                return StatusCode(500, "Đã xảy ra lỗi trong quá trình đặt phòng.");
            }
        }

    }
}
