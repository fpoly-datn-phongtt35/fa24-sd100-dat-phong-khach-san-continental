using Domain.DTO.Order;
using Domain.DTO.Paging;
using Domain.DTO.PaymentHistory;
using Domain.DTO.RoomBooking;
using Domain.Enums;
using Domain.Models;
using Domain.Services.IServices;
using Domain.Services.IServices.IRoomBooking;
using Domain.Services.Services.RoomBooking;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace View.Controllers
{
    public class PaymentHistoryController : Controller
    {
        HttpClient _client;
        IPaymentHistoryService _paymentHistoryService;
        IRoomBookingGetService _roomBookingGetService;
        IRoomBookingUpdateService _roomBookingUpdateService;
 
        public PaymentHistoryController(HttpClient client, IPaymentHistoryService paymentHistoryService, IRoomBookingGetService roomBookingGetService, IRoomBookingUpdateService roomBookingUpdateService )
        {
            _client = client;
            _paymentHistoryService = paymentHistoryService;
            _client.BaseAddress = new Uri("https://localhost:7130/");
            _roomBookingGetService = roomBookingGetService;
            _roomBookingUpdateService = roomBookingUpdateService;
         }


        [Route("/PaymentHistory/Id={IdRoomBooking}")]
        public async Task<IActionResult> PaymentHistoryByBooking(Guid IdRoomBooking) 
        {
            try
            {
                string requestUrl = "api/PaymentHistory/GetListPaymentHistory";
                PaymentHistoryGetRequest request = new PaymentHistoryGetRequest() 
                {
                    RoomBookingId = IdRoomBooking
                };
                var jsonRequest = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                // lấy từ api để trỏ đến UpdatePaymentHistory
                try
                {
                    var response = await _client.PostAsync(requestUrl, content);

                    var responseString = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<ResponseData<PaymentHistory>>(responseString);

                    var roomBooking = await _roomBookingGetService.GetRoomBookingById(IdRoomBooking);
                    var totalPaid = await _paymentHistoryService.GetTotalPaidAmountByRoomBookingId(IdRoomBooking);
                    ViewBag.RoomBooking = roomBooking;
                    ViewBag.TotalPaid = totalPaid;
                    return View(data);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        


        [Route("/PaymentHistory/AddPayment/{IdRoomBooking}")]
        public async Task<IActionResult> AddPayment(Guid IdRoomBooking)
        {
            try
            {
                var roomBooking = await _roomBookingGetService.GetRoomBookingById(IdRoomBooking);
                var totalPaid = await _paymentHistoryService.GetTotalPaidAmountByRoomBookingId(IdRoomBooking);

                decimal amountToPay;
                string message;

                if (totalPaid == 0)
                {
                    amountToPay = (decimal)(roomBooking.TotalRoomPrice * 0.2m); 
                    message = $"Số tiền cần thanh toán là: {amountToPay}";
                }
                else
                {
                    amountToPay = (decimal)(roomBooking.TotalPrice - totalPaid);
                    message = $"Số tiền cần thanh toán là: {amountToPay}";
                }

                ViewBag.Message = message;
                ViewBag.AmountToPay = amountToPay;
                ViewBag.TotalPaid = totalPaid;
                ViewBag.RoomBooking = roomBooking;
                return View("AddPayment");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("/PaymentHistory/ProcessPayment")]
        public async Task<IActionResult> ProcessPayment(Guid RoomBookingId, decimal Amount, PaymentMethod PaymentMethod)
        {
            try
            {
                var totalPaid = await _paymentHistoryService.GetTotalPaidAmountByRoomBookingId(RoomBookingId);

                var paymentType = totalPaid == 0 ? PaymentType.Deposit : PaymentType.Bill;

                if (PaymentMethod == PaymentMethod.Cash)
                {
                    var paymentHistory = new PaymentHistoryCreateRequest
                    {
                        RoomBookingId = RoomBookingId,
                        Amount = Amount,
                        Note = paymentType,
                        PaymentTime = DateTime.Now,
                        PaymentMethod = PaymentMethod,
                        OrderCode = 0
                    };

                    var result = await _paymentHistoryService.AddPaymentHistory(paymentHistory);

                    if (result == 1)
                    {
                        return RedirectToAction("PaymentHistoryByBooking", new { IdRoomBooking = RoomBookingId });
                    }
                    else
                    {
                        return RedirectToAction("AddPayment", new { IdRoomBooking = RoomBookingId });
                    }
                }
                else if (PaymentMethod == PaymentMethod.BankTransfer)
                {
                     var requestData = new PaymentLinkCreateRequest
                    {
                        RoomBookingId = RoomBookingId,
                        Money = (int?)Amount,
                        PaymentType = paymentType
                    };

                     var apiUrl = "https://localhost:7130/api/Order/admin-create";
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

                    try
                    {
                        var response = await _client.PostAsync(apiUrl, jsonContent);
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
                    catch (Exception ex)
                    {
                        ViewBag.ErrorMessage = ex.Message;
                        return View("Error");
                    }
                }
                return RedirectToAction("AddPayment", new { IdRoomBooking = RoomBookingId });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 5, Guid? roomBookingId = null, Guid? customerId = null, PaymentType? note = null, decimal? amount = null, PaymentMethod? paymentMethod = null, decimal? fromAmount = null, decimal? toAmount = null)
        {
            string requestUrl = "api/PaymentHistory/GetListPaymentHistory";
            var request = new PaymentHistoryGetRequest
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                RoomBookingId = roomBookingId,
                CustomerId = customerId,
                Note = note,
                Amount = amount,
                PaymentMethod = paymentMethod,
                FromAmount = fromAmount,
                ToAmount = toAmount
            };

            var jsonRequest = JsonConvert.SerializeObject(request);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync(requestUrl, content);

                var responseString = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ResponseData<PaymentHistory>>(responseString);

                var roomBookingIds = new List<Guid>();
                foreach (var item in data.data)
                {
                    roomBookingIds.Add(item.RoomBookingId);
                }
                //lấy ra customername từ customerid trong mỗi roombooking của paymenthistory
                var customerInfos = new List<(Guid roomBookingId, string customerName)>();
                foreach (var item in roomBookingIds)
                {
                    string rbRequestUrl = $"api/RoomBooking/GetRoomBookingById?roomBookingId={item}";
                    var rbJsonRequest = JsonConvert.SerializeObject(new { Id = item });
                    var rbContent = new StringContent(rbJsonRequest, Encoding.UTF8, "application/json");
                    var rbResponse = await _client.PostAsync(rbRequestUrl, rbContent);
                    var rbResponseString = await rbResponse.Content.ReadAsStringAsync();
                    var rbData = JsonConvert.DeserializeObject<RoomBooking>(rbResponseString);

                    string cRequestUrl = $"api/Customer/GetCustomerById?Id={rbData.CustomerId}";
                    var cJsonRequest = JsonConvert.SerializeObject(new { Id = rbData.CustomerId });
                    var cContent = new StringContent(cJsonRequest, Encoding.UTF8, "application/json");
                    var cResponse = await _client.PostAsync(cRequestUrl, cContent);
                    var cResponseString = await cResponse.Content.ReadAsStringAsync();
                    var cData = JsonConvert.DeserializeObject<Customer>(cResponseString);

                    customerInfos.Add((item, cData.FirstName + " " + cData.LastName));
                }
                ViewBag.CustomerList = customerInfos;
                ViewBag.RoomBookingList = roomBookingIds;
                ViewBag.PaymentMethodList = Enum.GetValues(typeof(PaymentMethod));
                ViewBag.PaymentTypeList = Enum.GetValues(typeof(PaymentType));

                return View(data);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
