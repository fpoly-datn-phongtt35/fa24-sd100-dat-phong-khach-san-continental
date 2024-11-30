using Domain.DTO.Room;
using Domain.DTO.RoomBooking;
using Domain.DTO.RoomBookingDetail;
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
        public RoomBookingController(IRoombooking roomBookingRepo, 
            IRoomBookingDetail roomBookingDetailRepo, 
            IRoom roomRepo)
        {
            _roomBookingRepo = roomBookingRepo;
            _roomBookingDetailRepo = roomBookingDetailRepo;
            _roomRepo = roomRepo;
        }
        //public IActionResult ModalPartial(Guid roomId, RoomBookingDetailCreateRequest roomBookingDetailCreateRequest)
        //{
        //    var room = _roomRepo.GetRoomById(roomId);

        //    var model = new { Room = room, BookingDetails = roomBookingDetailCreateRequest };
        //    return PartialView("ModalPartial", model);
        //}

        [HttpPost]
        public async Task<IActionResult> RoomBooking(RoomBookingDetailCreateRequest roomBookingDetailCreateRequest)
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

                var roomBookingCreate = new RoomBookingCreateRequestForCustomer
                {
                    CustomerId = _UserLogin,
                    BookingType = Domain.Enums.BookingType.Online,
                    TotalPrice = room.Price,
                    TotalRoomPrice = room.Price,
                    TotalServicePrice = 0,
                    Status = Domain.Enums.RoomBookingStatus.PENDING,
                    StaffId = null,
                    CreatedBy = _UserLogin,
                    NewId = null
                };

                var roomBooking = await _roomBookingRepo.CreateRoomBooking(roomBookingCreate);
                roomBookingDetailCreateRequest.RoomId = room.Id;
                roomBookingDetailCreateRequest.RoomBookingId = roomBooking;
                roomBookingDetailCreateRequest.CreatedBy = _UserLogin;
                roomBookingDetailCreateRequest.Status = Domain.Enums.RoomBookingStatus.PENDING;
                roomBookingDetailCreateRequest.Deposit = room.Price * 20 / 100;
                var roomBookingDetail = await _roomBookingDetailRepo.CreateRoomBookingDetail(roomBookingDetailCreateRequest);
                var roomStatus = new RoomUpdateStatusRequest
                {
                    Id = room.Id,
                    Status = Domain.Enums.RoomStatus.OutOfOrder,
                    ModifiedBy = _UserLogin,
                    ModifiedTime = DateTime.Now
                };
                var updateRoomStatus = await _roomRepo.UpdateRoomStatus(roomStatus);
                return RedirectToAction("Index", "Home");
            }

            return View("Details", "Room");
        }

    }
}
