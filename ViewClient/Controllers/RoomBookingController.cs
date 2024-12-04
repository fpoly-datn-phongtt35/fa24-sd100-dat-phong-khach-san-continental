using Domain.DTO.Order;
using Domain.DTO.Room;
using Domain.DTO.RoomBooking;
using Domain.DTO.RoomBookingDetail;
using Domain.DTO.ServiceOrderDetail;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
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
        private readonly HttpClient _httpClient;
        public RoomBookingController(IRoombooking roomBookingRepo,
            IRoomBookingDetail roomBookingDetailRepo,
            IRoom roomRepo,
            HttpClient httpClient,
            IServiceOderDetail serviceOderDetailRepo)
        {
            _roomBookingRepo = roomBookingRepo;
            _roomBookingDetailRepo = roomBookingDetailRepo;
            _roomRepo = roomRepo;
            _httpClient = httpClient;
            _serviceOderDetailRepo = serviceOderDetailRepo;
        }
        //public IActionResult ModalPartial(Guid roomId, RoomBookingDetailCreateRequest roomBookingDetailCreateRequest)
        //{
        //    var room = _roomRepo.GetRoomById(roomId);

        //    var model = new { Room = room, BookingDetails = roomBookingDetailCreateRequest };
        //    return PartialView("ModalPartial", model);
        //}

        //public async Task<IActionResult> CreatePaymentLink()
        //{
        //    return View(new PaymentLinkCreateRequest());
        //}

        [HttpGet]
        public IActionResult CreatePaymentLink()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentLink(PaymentLinkCreateRequest request)
        {
            var apiUrl = "https://localhost:7130/api/Order/create";

            var requestData = new PaymentLinkCreateRequest
            {
                RoomBookingId = request.RoomBookingId,
                PaymentType = PaymentType.Deposit,
                Money = null
            };


            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(apiUrl, jsonContent);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<PaymentLinkResponse>(responseContent);

                    if (result != null && result.Error == 0)
                    {
                        return Redirect(result.Data);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = result?.Message ?? "Failed to create payment link.";
                        return View("Error");
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "API call failed.";
                    return View("Error");
                }
            }
            catch (System.Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }


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
                        Status = Domain.Enums.RoomBookingStatus.NEW,
                        StaffId = Guid.Parse("1CBA4323-1532-479C-810C-7D3B52214EE9"),
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
                    roomBookingDetailCreateRequest.Status = Domain.Enums.RoomBookingStatus.NEW;
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

                    return Json(new { success = true, roomBookingId = roomBooking });
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
