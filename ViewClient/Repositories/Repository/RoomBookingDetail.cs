using Domain.DTO.RoomBooking;
using Domain.DTO.RoomBookingDetail;
using Newtonsoft.Json;
using System.Data;
using ViewClient.Repositories.IRepository;

namespace ViewClient.Repositories.Repository
{
    public class RoomBookingDetail : IRoomBookingDetail
    {
        private readonly HttpClient _httpClient;

        public RoomBookingDetail(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Guid> CreateRoomBookingDetail(RoomBookingDetailCreateRequestForCustomer request)
        {
            string url = $"https://localhost:7130/api/RoomBookingDetail/BookingRoomDetail";
            var response = await _httpClient.PostAsJsonAsync(url, request);

            if (response.IsSuccessStatusCode)
            {
                var resultString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Guid>(resultString);

                if (result != Guid.Empty)
                {
                    return result;
                }
                else
                {
                    throw new Exception("Đặt phòng không thành công.");
                }
            }

            throw new Exception("Lỗi khi gọi API: " + response.ReasonPhrase);
        }

        public async Task<int> UpdateRoomBookingDetail(RoomBookingDetailUpdateRequest request)
        {
            string url = $"https://localhost:7130/api/RoomBookingDetail/UpdateRoomBookingDetail";
            var response = await _httpClient.PutAsJsonAsync(url, request);

            if (response.IsSuccessStatusCode)
            {
                var resultString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<int>(resultString);

                if (result != null && result == 1)
                {
                    return 1;
                }
                else
                {
                    throw new Exception("Cập nhật đặt phòng không thành công.");
                }
            }

            // Xử lý lỗi nếu cần
            throw new Exception("Lỗi khi gọi API: " + response.ReasonPhrase);
        }
    }
}
