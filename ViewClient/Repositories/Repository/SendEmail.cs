using Domain.DTO.Email;
using Newtonsoft.Json;
using ViewClient.Repositories.IRepository;

namespace ViewClient.Repositories.Repository
{
    public class SendEmail : ISendEmail
    {
        private readonly HttpClient _httpClient;

        public SendEmail(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> SendAccountAsync(AccountRequest request)
        {
            string url = $"https://localhost:7130/api/Email/SendAccount";
            var response = await _httpClient.PostAsJsonAsync(url, request);
            if (response.IsSuccessStatusCode)
            {
                var resultString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<int>(resultString);
                return result;
            }
            return 0;
        }

        public async Task<int> SendEmailAsync(EmailRequest request)
        {
            string url = $"https://localhost:7130/api/Email/send-email";
            var response = await _httpClient.PostAsJsonAsync(url, request);
            if (response.IsSuccessStatusCode)
            {
                var resultString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<int>(resultString);
                return result;
            }
            return 0;
        }
    }
}
