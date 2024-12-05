using Domain.DTO.Paging;
using Domain.DTO.PaymentHistory;
using Domain.DTO.RoomBooking;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace View.Controllers
{
    public class PaymentHistoryController : Controller
    {
        HttpClient _client;

        public PaymentHistoryController(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://localhost:7130/");
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
