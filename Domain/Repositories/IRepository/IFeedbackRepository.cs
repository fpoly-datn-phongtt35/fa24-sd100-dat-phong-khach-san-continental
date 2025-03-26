using Domain.DTO.Feedback;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.IRepository
{
    public interface IFeedbackRepository
    {
        Task<DataTable> GetAllFeedbacks(FeedbackGetRequest request);
        Task<DataTable> GetFeedbackById(Guid id);
        Task<int> AddFeedback (FeedbackCreateRequest request);
        Task<int> UpdateFeedback(FeedbackUpdateRequest request);
        Task<int> DeleteFeedback(FeedbackDeleteRequest request);
    }
}
