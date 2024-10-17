using Domain.DTO.Paging;
using Domain.DTO.Service;
using Domain.DTO.ServiceOrderDetail;
using Domain.DTO.Voucher;
using Domain.DTO.VoucherDetail;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace View.Controllers
{
    public class VoucherDetailController : Controller
    {
        HttpClient _client;

        public VoucherDetailController(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://localhost:7130/");
        }
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 5, Guid? roomBookingId = null, Guid? voucherId = null)
        {
            string requestUrl = "api/VoucherDetail/GetListVoucherDetail";

            var request = new VoucherDetailGetRequest
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                RoomBookingId = roomBookingId,
                VoucherId = voucherId
            };

            var jsonRequest = JsonConvert.SerializeObject(request);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync(requestUrl, content);

                var responseString = await response.Content.ReadAsStringAsync();

                var voucherDetails = JsonConvert.DeserializeObject<ResponseData<VoucherDetail>>(responseString);

                ViewBag.VoucherList = await GetVoucherList();

                return View(voucherDetails);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        // GET: ServiceOrderDetailController/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.VoucherList = await GetVoucherList();
            return View(new VoucherDetailCreateRequest());
        }

        // POST: ServiceOrderDetailController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VoucherDetailCreateRequest request)
        {
            if (ModelState.IsValid)
            {
                request.Status = EntityStatus.Active;
                request.CreatedTime = DateTimeOffset.Now;
                var response = await _client.PostAsJsonAsync("api/VoucherDetail/AddVoucherDetail", request);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(request);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            string requestUrl = $"api/VoucherDetail/GetVoucherDetailById?id={id}";

            var jsonRequest = JsonConvert.SerializeObject(new { Id = id });
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync(requestUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }

                var responseString = await response.Content.ReadAsStringAsync();
                var voucher = JsonConvert.DeserializeObject<VoucherDetail>(responseString);



                return View(voucher);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: VoucherDetailController/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            // Lấy thông tin VoucherDetail hiện tại
            string requestUrl = $"api/VoucherDetail/GetVoucherDetailById?id={id}";

            var jsonRequest = JsonConvert.SerializeObject(new { Id = id });
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(requestUrl, content);

            var responseString = await response.Content.ReadAsStringAsync();
            var voucherDetail = JsonConvert.DeserializeObject<VoucherDetail>(responseString);

            ViewBag.VoucherList = await GetVoucherList();
            ViewBag.Statuses = Enum.GetValues(typeof(EntityStatus));

            return View(voucherDetail);
        }


        // POST: VoucherDetailController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VoucherDetail request)
        {
            request.ModifiedTime = DateTimeOffset.Now;
            await _client.PutAsJsonAsync("api/VoucherDetail/UpdateVoucherDetail", request);
            return RedirectToAction("Index");
        }

        private async Task<List<Voucher>> GetVoucherList()
        {
            string voucherRequestURL = "api/Voucher/GetListVoucher";

            var voucherRequest = new VoucherGetRequest();

            var voucherJsonRequest = JsonConvert.SerializeObject(voucherRequest);

            var voucherContent = new StringContent(voucherJsonRequest, Encoding.UTF8, "application/json");

            var voucherResponse = await _client.PostAsync(voucherRequestURL, voucherContent);
            var voucherResponseString = await voucherResponse.Content.ReadAsStringAsync();

            var vouchers = JsonConvert.DeserializeObject<ResponseData<Voucher>>(voucherResponseString);

            return vouchers.data;
        }

        // GET: ServiceOrderDetailController/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            string requestUrl = "api/VoucherDetail/DeleteVoucherDetail";

            var request = new VoucherDetailDeleteRequest
            {
                Id = id,
                DeletedBy = Guid.NewGuid(),
                DeletedTime = DateTimeOffset.Now
            };

            var jsonRequest = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            await _client.PostAsync(requestUrl, content);

            return RedirectToAction("Index");
        }
    }
}
