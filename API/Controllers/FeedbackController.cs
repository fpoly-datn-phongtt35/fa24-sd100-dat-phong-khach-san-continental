using Domain.DTO.Feedback;
using Domain.DTO.Paging;
using Domain.Models;
using Domain.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _service;

        public FeedbackController(IFeedbackService service)
        {
            _service = service;
        }

        [HttpPost("CreateFeedback")]
        public async Task<int> CreateFeedback(FeedbackCreateRequest request)
        {
            try
            {
                return await _service.AddFeedback(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("DeleteFeedback")]
        public async Task<int> DeleteFeedback(FeedbackDeleteRequest request)
        {
            try
            {
                return await _service.DeleteFeedback(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("GetFeedbackById")]
        public async Task<FeedbackDto> GetFeedbackById(Guid id)
        {
            try
            {
                return await _service.GetFeedbackById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("GetFeedbacks")]
        public async Task<ResponseData<FeedbackDto>> GetFeedbacks(FeedbackGetRequest request)
        {
            try
            {
                return await _service.GetListFeedback(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("UpdateFeedback")]
        public async Task<int> UpdateFeedback(FeedbackUpdateRequest request)
        {
            try
            {
                return await _service.UpdateFeedback(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
