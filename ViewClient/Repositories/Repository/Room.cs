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
    }
}
