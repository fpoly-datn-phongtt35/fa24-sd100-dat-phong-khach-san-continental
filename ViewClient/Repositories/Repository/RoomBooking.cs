using Domain.DTO.RoomBooking;
using Newtonsoft.Json;
using System.Data;
using ViewClient.Repositories.IRepository;

namespace ViewClient.Repositories.Repository
{
    public class RoomBooking : IRoombooking
    {
        private readonly HttpClient _httpClient;

        public RoomBooking(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> CreateRoomBooking(RoomBookingCreateRequest request)
        {
            string url = $"https://localhost:7130/api/RooomBooking/CreateRoomBooking";
            var response = await _httpClient.PostAsJsonAsync(url, request);

            if (response.IsSuccessStatusCode)
            {
                var resultString = await response.Content.ReadAsStringAsync();
                var dataTable = JsonConvert.DeserializeObject<DataTable>(resultString);

                // Kiểm tra nếu DataTable có ít nhất một hàng
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    // Giả sử cột đầu tiên chứa ID của booking
                    return Convert.ToInt32(dataTable.Rows[0][0]);
                }
                else
                {
                    throw new Exception("Không có dữ liệu trả về từ API.");
                }
            }

            // Xử lý lỗi nếu cần
            throw new Exception("Lỗi khi gọi API: " + response.ReasonPhrase);
        }
    }
}
