using Domain.DTO.Paging;
using Domain.DTO.ResidenceRegistration;
using Domain.Models;
using Domain.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace View.Controllers
{
    public class ResidenceRegistrationController : Controller
    {
        private readonly HttpClient _client;
        private readonly IResidenceRegistrationService _residenceRegistrationService;

        public ResidenceRegistrationController(IResidenceRegistrationService residenceRegistrationService, HttpClient client)
        {
            _residenceRegistrationService = residenceRegistrationService;
            _client = client;
            _client.BaseAddress = new Uri("https://localhost:7130/");
        }

        public async Task<IActionResult> GetResidenceByDate(int pageIndex = 1, int pageSize = 10, DateTime? date = null, string? fullName = null, string? identityNumber = null, string? roomName = null, bool? isCheckOut = null)
        {
            string requestUrl = "api/ResidenceRegistration/GetResidenceRegistrationsByDate";

            var request = new ResidenceGetByDateRequest
            {
                Date = date,
                PageIndex = pageIndex,
                PageSize = pageSize,
                FullName = fullName,
                IdentityNumber = identityNumber,
                RoomName = roomName,
                IsCheckOut = isCheckOut
            };
            var jsonRequest = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync(requestUrl, content);
                var responseString = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ResponseData<ResidenceResponse>>(responseString);
                return View(data);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        public async Task<List<ResidenceRegistration>> GetResidenceRegistrationByRoomBookingDetailId(Guid Id)
        {
            try
            {
                var model = await _residenceRegistrationService.GetResidenceByRoomBookingDetailId(Id);
 
                return model.data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> CheckOut1Residence(Guid id)
        {
            try
            {
                var result = await _residenceRegistrationService.CheckOut1Residence(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IActionResult> CheckOutResideecByRBD(Guid roomBookingDetailId, DateTimeOffset outTime)
        {
            try
            {
                // Kiểm tra kiểu dữ liệu của outTime và chuyển đổi nếu cần thiết
                if (outTime == null || outTime == DateTimeOffset.MinValue) // hoặc bất kỳ kiểm tra giá trị không hợp lệ nào
                {
                    // Giả sử bạn nhận giá trị outTime từ phía client là kiểu string hoặc DateTime và cần phải chuyển đổi nó
                    string outTimeString = outTime.ToString(); // Lấy giá trị string của outTime nếu có

                    if (DateTimeOffset.TryParse(outTimeString, out DateTimeOffset parsedOutTime))
                    {
                        outTime = parsedOutTime;  // Cập nhật outTime bằng giá trị đã chuyển đổi
                    }
                    else
                    {
                        // Nếu không thể chuyển đổi, bạn có thể xử lý ngoại lệ hoặc trả về lỗi
                        return BadRequest("Invalid check-out time format.");
                    }
                }

                // Gọi service để xử lý check-out
                var result = await _residenceRegistrationService.CheckOutResidenceByRBD(roomBookingDetailId, outTime);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log và xử lý ngoại lệ nếu cần
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        public async Task<IActionResult> GetMaximumOccupancyByRoomBookingDetailId(Guid Id)
        {
            try
            {
                var result = await _residenceRegistrationService.GetMaximumOccupancyByRoomBookingDetailId(Id);
                return Ok(new { maximumOccupancy = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddResidenceRegistration(ResidenceAddRequest request)
        {
            var obj = new ResidenceAddRequest
            {
                RoomBookingDetailId = request.RoomBookingDetailId,
                FullName = request.FullName,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                IdentityNumber = request.IdentityNumber,
                PhoneNumber = request.PhoneNumber,
                //CreatedBy = null,
                //CreatedTime = DateTimeOffset.Now
            };
            try
            {
                var result = await _residenceRegistrationService.AddResidence(obj);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteResidenceRegistration(Guid id)
        {
            try
            {
                var result = await _residenceRegistrationService.DeleteResidence(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditResidenceRegistration(ResidenceUpdateRequest request)
        {
            try
            {
                var result = await _residenceRegistrationService.UpdateResidence(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
