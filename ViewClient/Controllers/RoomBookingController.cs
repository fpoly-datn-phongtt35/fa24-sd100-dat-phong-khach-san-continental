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
        public RoomBookingController(IRoombooking roomBookingRepo, 
            IRoomBookingDetail roomBookingDetailRepo, 
            IRoom roomRepo,
            IServiceOderDetail serviceOderDetailRepo)
        {
            _roomBookingRepo = roomBookingRepo;
            _roomBookingDetailRepo = roomBookingDetailRepo;
            _roomRepo = roomRepo;
            _serviceOderDetailRepo = serviceOderDetailRepo;
        }
        //public IActionResult ModalPartial(Guid roomId, RoomBookingDetailCreateRequest roomBookingDetailCreateRequest)
        //{
        //    var room = _roomRepo.GetRoomById(roomId);

        //    var model = new { Room = room, BookingDetails = roomBookingDetailCreateRequest };
        //    return PartialView("ModalPartial", model);
        //}

        [HttpPost]
        public async Task<IActionResult> RoomBooking([FromBody] RoomBookingDetailCreateRequest roomBookingDetailCreateRequest)
        {
            try
            {
                if (HttpContext.Session.GetString("UserName") == null)
                {
                    TempData["ErrorMessage"] = "Vui lòng đăng nhập để đặt phòng.";
                    return RedirectToAction("Login", "Authoration");
                }

                var _UserLogin = Guid.Empty;
                _UserLogin = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.UserData).Value);

                if (_UserLogin != Guid.Empty)
                {
                    var room = await _roomRepo.GetRoomById(roomBookingDetailCreateRequest.RoomId);
                    if (room == null)
                    {
                        return NotFound();
                    }

                    // Tạo đối tượng đặt phòng
                    var roomBookingCreate = new RoomBookingCreateRequestForCustomer
                    {
                        CustomerId = _UserLogin,
                        BookingType = Domain.Enums.BookingType.Online,
                        TotalPrice = room.Price,
                        TotalRoomPrice = room.Price,
                        TotalServicePrice = 0,
                        Status = Domain.Enums.EntityStatus.Active,
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
                    roomBookingDetailCreateRequest.Status = Domain.Enums.EntityStatus.Active;
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

                return View("Details", "Room");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Đã xảy ra lỗi trong quá trình đặt phòng.");
            }
        }

    }
}
