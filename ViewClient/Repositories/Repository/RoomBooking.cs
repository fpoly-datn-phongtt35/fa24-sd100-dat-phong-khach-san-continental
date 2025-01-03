using Domain.DTO.Paging;
using Domain.DTO.RoomBooking;
using Newtonsoft.Json;
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

        public async Task<Guid> CreateRoomBooking(RoomBookingCreateRequestForCustomer request)
        {
            string url = $"https://localhost:7130/api/RoomBooking/CreateRoomBookingForCustomer";
            var response = await _httpClient.PostAsJsonAsync(url, request);

            if (response.IsSuccessStatusCode)
            {
                var resultString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Guid>(resultString);

                if (result != null)
                {
                    return result;
                }
                else
                {
                    throw new Exception("Đặt phòng không thành công.");
                }
            }

            // Xử lý lỗi nếu cần
            throw new Exception("Lỗi khi gọi API: " + response.ReasonPhrase);
        }

        public async Task<ResponseData<RoomBookingResponseForCustomer>> GetListRoomBookingByCustomerId(RoomBookingGetRequestByCustomer request)
        {
            string url = $"https://localhost:7130/api/RoomBooking/GetListRoomBookingByCustomerId";
            var response = await _httpClient.PostAsJsonAsync(url, request);

            if (response.IsSuccessStatusCode)
            {
                var resultString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ResponseData<RoomBookingResponseForCustomer>>(resultString);

                if (result != null)
                {
                    return result;
                }
                else
                {
                    throw new Exception("Lấy danh sách phòng không thành công.");
                }
            }

            // Xử lý lỗi nếu cần
            throw new Exception("Lỗi khi gọi API: " + response.ReasonPhrase);
        }
    }
}
