using Domain.DTO.ServiceOrderDetail;
using Newtonsoft.Json;
using ViewClient.Repositories.IRepository;
namespace ViewClient.Repositories.Repository
{
    public class ServiceOrderDetail : IServiceOderDetail
    {
        private readonly HttpClient _httpClient;

        public ServiceOrderDetail(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> AddServiceOrderDetail(ServiceOrderDetailCreateRequest request)
        {
            string url = $"https://localhost:7130/api/ServiceOrderDetail/AddServiceOrderDetail";
            var response = await _httpClient.PostAsJsonAsync(url, request);

            if (response.IsSuccessStatusCode)
            {
                var resultString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Guid>(resultString);

                if (result != null)
                {
                    return 1;
                }
                else
                {
                    throw new Exception("Đặt dịch vụ không thành công.");
                }
            }

            // Xử lý lỗi nếu cần
            throw new Exception("Lỗi khi gọi API: " + response.ReasonPhrase);
        }
    }
}
