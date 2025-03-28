using Domain.DTO.Feedback;
using Domain.DTO.Paging;
using ViewClient.Repositories.IRepository;

namespace ViewClient.Repositories.Repository
{
    public class Feedback : IFeedback
    {
        private readonly HttpClient _httpClient;

        public Feedback(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseData<FeedbackDto>> GetListFeedbacks(FeedbackGetRequest request)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("https://localhost:7130/api/Feedback/GetFeedbacks", request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ResponseData<FeedbackDto>>();
            }
            return null;
        }

        public async Task<int> AddFeedback(FeedbackCreateRequest request)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("https://localhost:7130/api/Feedback/CreateFeback", request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<int>();
            }
            return -1;
        }
    }
}
