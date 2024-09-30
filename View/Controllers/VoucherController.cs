using Domain.DTO.Customer;
using Domain.DTO.Paging;
using Domain.DTO.Voucher;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace View.Controllers
{
	public class VoucherController : Controller
	{
		private readonly HttpClient _httpClient;

		public VoucherController()
		{
			_httpClient = new HttpClient();
		}

		public async Task<ActionResult> Index()
		{
			string requestURL = "https://localhost:7130/api/Voucher/GetListVoucher";

			var voucher = new VoucherGetRequest();

			var jsonRequest = JsonConvert.SerializeObject(voucher);

			var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
			try
			{
				// gửi request lên api
				var response = await _httpClient.PostAsync(requestURL, content);

				// đọc nội dung trả về từ api
				var responseString = await response.Content.ReadAsStringAsync();

				// chuyển đổi lại thành respondata 
				var vouchers = JsonConvert.DeserializeObject<ResponseData<Voucher>>(responseString);

				return View(vouchers);
			}
			catch (Exception ex)
			{
				return View("Error", ex);
			}
		}

		public async Task<IActionResult> Details(Guid id)
		{
			string requestUrl = $"https://localhost:7130/api/Voucher/GetVoucherById?Id={id}";

			// Tạo nội dung json cho request
			var jsonRequest = JsonConvert.SerializeObject(new { Id = id });
			var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

			try
			{
				var response = await _httpClient.PostAsync(requestUrl, content);

				if (!response.IsSuccessStatusCode)
				{
					return View("Error");
				}

				var responseString = await response.Content.ReadAsStringAsync();
				var voucher = JsonConvert.DeserializeObject<Voucher>(responseString);

				return View(voucher);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
		public async Task<IActionResult> Create()
		{
			ViewBag.Statuses = Enum.GetValues(typeof(EntityStatus));
			ViewBag.DiscountTypies = Enum.GetValues(typeof(DiscountType));
			return View(new VoucherCreateRequest());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(VoucherCreateRequest request)
		{
			if (ModelState.IsValid)
			{
				string requestURL = "https://localhost:7130/api/Voucher/CreateVoucher";
				request.Status = EntityStatus.Active;
				request.CreatedTime = DateTimeOffset.Now;
				var response = await _httpClient.PostAsJsonAsync(requestURL, request);

				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction("Index");
				}
			}
			return View(request);
		}
		public async Task<IActionResult> Edit(Guid id)
		{

			string requestUrl = $"https://localhost:7130/api/Voucher/GetVoucherById?Id={id}";

			var jsonRequest = JsonConvert.SerializeObject(new { Id = id });
			var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            ViewBag.DiscountTypies = Enum.GetValues(typeof(DiscountType));
            ViewBag.Statuses = Enum.GetValues(typeof(EntityStatus));

			try
			{
				var response = await _httpClient.PostAsync(requestUrl, content);

				if (!response.IsSuccessStatusCode)
				{
					return View("Error");
				}

				var responseString = await response.Content.ReadAsStringAsync();
				var voucher = JsonConvert.DeserializeObject<Voucher>(responseString);



				return View(voucher);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost]
		public async Task<IActionResult> Edit(Voucher request)
		{
            ViewBag.DiscountTypies = Enum.GetValues(typeof(DiscountType));
            ViewBag.Statuses = Enum.GetValues(typeof(EntityStatus));

			request.ModifiedTime = DateTimeOffset.Now;
			var response = await _httpClient.PutAsJsonAsync("https://localhost:7130/api/Voucher/UpdateVoucher", request);
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Delete(Guid id)
		{
			string requestUrl = "https://localhost:7130/api/Voucher/DeleteVoucher";

			var request = new VoucherDeleteRequest
			{
				Id = id,
				DeletedBy = Guid.NewGuid(),
				DeletedTime = DateTimeOffset.Now
			};

			var jsonRequest = JsonConvert.SerializeObject(request);
			var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync(requestUrl, content);

			if (response.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");
			}

			ModelState.AddModelError("", "Unable to delete the voucher.");
			return View("Error", new Exception("Unable to delete the voucher."));
		}
	}
}
