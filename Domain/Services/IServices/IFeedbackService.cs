using Domain.DTO.Feedback;
using Domain.DTO.Paging;
using Domain.Models;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.IServices
{
    public interface IFeedbackService
    {
        Task<int> AddFeedback(FeedbackCreateRequest request);
        Task<int> DeleteFeedback(FeedbackDeleteRequest request);
        Task<int> UpdateFeedback(FeedbackUpdateRequest request);
        Task<ResponseData<FeedbackDto>> GetListFeedback(FeedbackGetRequest request);
        Task<FeedbackDto> GetFeedbackById(Guid id);
    }
}
