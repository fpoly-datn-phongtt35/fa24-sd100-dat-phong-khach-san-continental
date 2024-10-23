using Domain.DTO.Paging;
using Domain.DTO.Staff;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace View.Controllers.Staff.Service
{
    public class StaffServices
    {
        private readonly HttpClient _httpClient;
        public StaffServices()
        {
            _httpClient = new HttpClient();
        }

        public async Task<ResponseData<Domain.Models.Staff>> GetListData(StaffGetRequest request)
        {
            string requestURL = "https://localhost:7130/api/Staff/GetListStaff";

            var jsonRequest = JsonConvert.SerializeObject(request);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            try
            {
                // gửi request lên api
                var response = await _httpClient.PostAsync(requestURL, content);

                // đọc nội dung trả về từ api
                var responseString = await response.Content.ReadAsStringAsync();

                // chuyển đổi lại thành respondata 
                var staffs = JsonConvert.DeserializeObject<ResponseData<Domain.Models.Staff>>(responseString);

                return staffs;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
