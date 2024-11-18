using Domain.DTO.Room;
using Domain.DTO.RoomBooking;
using Domain.DTO.RoomBookingDetail;
using Domain.Models;
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
        public IActionResult ModalPartial(Guid roomId, RoomBookingDetailCreateRequest roomBookingDetailCreateRequest)
        {
            var room = _roomRepo.GetRoomById(roomId);

            var model = new { Room = room, BookingDetails = roomBookingDetailCreateRequest };
            return PartialView("ModalPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> RoomBooking(RoomBookingDetailCreateRequest roomBookingDetailCreateRequest)
        {
            var _UserLogin = Guid.Empty;
            if (HttpContext.User.FindFirst(ClaimTypes.UserData) != null)
            {
                _UserLogin = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.UserData).Value);
                if (_UserLogin != Guid.Empty)
                {
                    var roomBookingCreate = new RoomBookingCreateRequestForCustomer
                    {
                        CustomerId = _UserLogin,
                        BookingType = Domain.Enums.BookingType.Online,
                        Status = Domain.Enums.EntityStatus.Active,
                        StaffId = null,
                        CreatedBy = _UserLogin
                    };
                    var roomBooking = await _roomBookingRepo.CreateRoomBooking(roomBookingCreate);

                    //var room = _roomRepo.GetRoomById(roomId);

                    //RoomId = roomBookingDetailCreateRequest.RoomId;
                    roomBookingDetailCreateRequest.RoomBookingId = roomBooking;
                    //Price = room.Price;
                    var roomBookingDetail = await _roomBookingDetailRepo.CreateRoomBookingDetail(roomBookingDetailCreateRequest);

                    //var room = await _roomRepo.UpdateRoomStatus();
                    return View("Index","Home");
                }
                
            }

            return View();
        }

    }
}
