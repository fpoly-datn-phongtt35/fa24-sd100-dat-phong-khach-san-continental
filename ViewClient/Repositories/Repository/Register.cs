using Newtonsoft.Json;
using System.Net.Http;
using ViewClient.Models.DTO.Login;
using ViewClient.Models.DTO.Register;
using ViewClient.Repositories.IRepository;

namespace ViewClient.Repositories.Repository
{
    public class Register :IRegister
    {
        private readonly HttpClient _httpClient;

        public Register(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ClientAuthenicationViewModel> RegisterAsync(RegisterInputRequest request)
        {
            string url = $"https://localhost:7130/api/Register";
            var response = await _httpClient.PostAsJsonAsync(url, request);
            if (response.IsSuccessStatusCode)
            {
                var resultString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ClientAuthenicationViewModel>(resultString);
                return result;
            }

            // Xử lý lỗi nếu cần
            return null;
        }
    }
}
