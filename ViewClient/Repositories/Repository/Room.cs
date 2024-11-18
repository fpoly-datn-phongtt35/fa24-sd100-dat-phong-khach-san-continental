using Domain.DTO.Paging;
using Domain.DTO.Room;
using Newtonsoft.Json;
using ViewClient.Repositories.IRepository;

namespace ViewClient.Repositories.Repository
{
    public class Room : IRoom
    {
        private readonly HttpClient _httpClient;

        public Room(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseData<RoomResponse>> GetAllRooms(RoomRequest roomRequest)
        {
            string url = $"https://localhost:7130/api/Room/GetAllRooms";
            var response = await _httpClient.PostAsJsonAsync(url, roomRequest);
            if (response.IsSuccessStatusCode)
            {
                var resultString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ResponseData<RoomResponse>>(resultString);
                Console.WriteLine(result);
                return result;
            }

            // Xử lý lỗi nếu cần
            return null;
        }

        public async Task<RoomResponse?> GetRoomById(Guid roomId)
        {
            string url = $"https://localhost:7130/api/Room/GetRoomById?roomId={roomId}";
            var response = await _httpClient.PostAsJsonAsync(url, roomId);
            if (response.IsSuccessStatusCode)
            {
                var resultString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<RoomResponse>(resultString);
                return result;
            }
            return null;
        }

        public async Task<int?> UpdateRoomStatus(RoomUpdateStatusRequest request)
        {
            string url = $"https://localhost:7130/api/Room/UpdateRoomStatus";
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
                    throw new Exception("Cập nhật phòng không thành công.");
                }
            }

            // Xử lý lỗi nếu cần
            throw new Exception("Lỗi khi gọi API: " + response.ReasonPhrase);
        }
    }
}
