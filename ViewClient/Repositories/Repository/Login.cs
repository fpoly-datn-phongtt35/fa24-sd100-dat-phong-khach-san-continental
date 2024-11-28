using Newtonsoft.Json;
using ViewClient.Models.DTO.Login;
using ViewClient.Repositories.IRepository;

namespace ViewClient.Repositories.Repository
{
    public class Login : ILogin
    {
        private readonly HttpClient _httpClient;

        public Login(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ClientAuthenicationViewModel> LoginAsync(LoginInputRequest request)
        {
            string url = $"https://localhost:7130/api/Login";
            var response = await _httpClient.PostAsJsonAsync(url, request);

            if (response.IsSuccessStatusCode)
            {
                var resultString = await response.Content.ReadAsStringAsync();
                Console.WriteLine(resultString); // Ghi lại phản hồi JSON

                if (string.IsNullOrEmpty(resultString))
                {
                    return null; 
                }

                try
                {
                    var result = JsonConvert.DeserializeObject<ClientAuthenicationViewModel>(resultString);
                    return result;
                }
                catch (JsonSerializationException ex)
                {
                    Console.WriteLine($"Lỗi deserialization: {ex.Message}");
                    return null;
                }
            }

            var errorString = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Phản hồi lỗi: {errorString}");
            return null;
        }
    }
}
