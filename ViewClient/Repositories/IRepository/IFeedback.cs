using Domain.DTO.Feedback;
using Domain.DTO.Paging;

namespace ViewClient.Repositories.IRepository
{
    public interface IFeedback
    {
        Task<ResponseData<FeedbackDto>> GetListFeedbacks(FeedbackGetRequest request);
        Task<int> AddFeedback(FeedbackCreateRequest request);
    }
}
