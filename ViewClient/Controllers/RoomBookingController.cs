using Domain.DTO.Customer;
using Domain.DTO.Order;
using Domain.DTO.Paging;
using Domain.DTO.Room;
using Domain.DTO.RoomBooking;
using Domain.DTO.RoomBookingDetail;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using Utilities;
using ViewClient.Repositories.IRepository;

namespace ViewClient.Controllers
{
    public class RoomBookingController : Controller
    {
        private readonly IRoombooking _roomBookingRepo;
        private readonly IRoomBookingDetail _roomBookingDetailRepo;
        private readonly IRoom _roomRepo;
        private readonly IServiceOderDetail _serviceOderDetailRepo;
        private readonly ICustomer _customerRepo;
        private readonly HttpClient _httpClient;
        public RoomBookingController(IRoombooking roomBookingRepo,
            IRoomBookingDetail roomBookingDetailRepo,
            IRoom roomRepo,
            ICustomer customerRepo,
            HttpClient httpClient,
            IServiceOderDetail serviceOderDetailRepo)
        {
            _roomBookingRepo = roomBookingRepo;
            _roomBookingDetailRepo = roomBookingDetailRepo;
            _roomRepo = roomRepo;
            _httpClient = httpClient;
            _serviceOderDetailRepo = serviceOderDetailRepo;
            _customerRepo = customerRepo;
        }
        [HttpGet]
        public async Task<IActionResult> BookingHistory(RoomBookingGetRequestByCustomer request)
        {
            try
            {
                var _UserLogin = Guid.Empty;
                if (HttpContext.User.FindFirst(ClaimTypes.UserData) != null)
                {
                    _UserLogin = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.UserData).Value);
                }

                if (_UserLogin == Guid.Empty)
                {
                    return StatusCode(401, new { error = "Bạn cần đăng nhập để thực hiện chức năng này." });
                }

                request.CustomerId = _UserLogin;
                ViewBag.UserLogin = request.CustomerId;
                var roomBookings = await _roomBookingRepo.GetListRoomBookingByCustomerId(request);
                return View(roomBookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Đã xảy ra lỗi trong quá trình lấy dữ liệu.");
            }
        }
        
        [HttpGet]
        public IActionResult CreatePaymentLink()
        {
            return View();
        }

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
        public async Task<IActionResult> RoomBooking(RoomBookingDetailCreateRequest roomBookingDetailCreateRequest)
        {
            try
            {
                var _UserLogin = Guid.Empty;
                if (HttpContext.User.FindFirst(ClaimTypes.UserData) != null)
                {
                    _UserLogin = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.UserData).Value);
                }

                Guid customerId = _UserLogin;
                if (_UserLogin == Guid.Empty)
                {
                    string[] parts = roomBookingDetailCreateRequest.Customer!.Email.Split('@');
                    var customer = new ClientCreateCustomerRequest
                    {
                        UserName = parts[0],
                        Password = PasswordHashingHelper.RandomPassword(),
                        FirstName = roomBookingDetailCreateRequest.Customer.FirstName,
                        LastName = roomBookingDetailCreateRequest.Customer.LastName,
                        Email = roomBookingDetailCreateRequest.Customer.Email,
                        PhoneNumber = roomBookingDetailCreateRequest.Customer.PhoneNumber,
                        CreatedTime = DateTimeOffset.Now
                    };
                    var insertCustomer = await _customerRepo.ClientInsertCustomer(customer);
                    customerId = insertCustomer.Id;
                    if (customerId == Guid.Empty)
                    {
                        return StatusCode(422, new { error = "Thông tin của bạn cần chính xác.", message = insertCustomer.Messenger });
                    }
                }
                var room = await _roomRepo.GetRoomById(roomBookingDetailCreateRequest.RoomId);
                if (room == null)
                {
                    return NotFound();
                }
                // Tạo đối tượng đặt phòng
                var roomBookingCreate = new RoomBookingCreateRequestForCustomer
                {
                    CustomerId = customerId,
                    BookingType = BookingType.Online,
                    TotalPrice = Math.Round(roomBookingDetailCreateRequest.Price + roomBookingDetailCreateRequest.ServicePrice - roomBookingDetailCreateRequest.Deposit ?? 0),
                    TotalRoomPrice = Math.Round(roomBookingDetailCreateRequest.Price ?? 0),
                    TotalServicePrice = roomBookingDetailCreateRequest.ServicePrice,
                    Status = RoomBookingStatus.PENDING,
                    StaffId = null,
                    CreatedBy = customerId,
                    NewId = null,
                    TotalExpenses = 0,
                    TotalPriceReality = 0,
                    TotalExtraPrice = 0,
                    BookingBy = BookingBy.Day
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
                            Status = EntityStatus.Active,
                            CreatedTime = DateTime.Now,
                            CreatedBy = customerId,
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
                roomBookingDetailCreateRequest.CreatedBy = customerId;
                roomBookingDetailCreateRequest.Status = RoomBookingStatus.PENDING;
                roomBookingDetailCreateRequest.Deposit = roomBookingDetailCreateRequest.Deposit;
                roomBookingDetailCreateRequest.Price = roomBookingDetailCreateRequest.Price;
                roomBookingDetailCreateRequest.ExtraPrice = 0;
                roomBookingDetailCreateRequest.SelectedServices = null;
                roomBookingDetailCreateRequest.Customer = null;
                roomBookingDetailCreateRequest.ServicePrice = null;
                roomBookingDetailCreateRequest.TotalPrice = null;
                roomBookingDetailCreateRequest.Expenses = null;
                roomBookingDetailCreateRequest.Note = null;
                var roomBookingDetail = await _roomBookingDetailRepo.CreateRoomBookingDetail(roomBookingDetailCreateRequest);

                var roomStatus = new RoomUpdateStatusRequest
                {
                    Id = room.Id,
                    Status = RoomStatus.AwaitingConfirmation,
                    ModifiedBy = customerId,
                    ModifiedTime = DateTime.Now
                };
                await _roomRepo.UpdateRoomStatus(roomStatus);

                //return RedirectToAction("Index", "Home");
                return Json(new { success = true, roomBookingId = roomBooking });
            }
            catch (Exception ex)
            {
                return View("Details", "Room");
                //return StatusCode(500, "Đã xảy ra lỗi trong quá trình đặt phòng.");
            }
        }

    }
}
