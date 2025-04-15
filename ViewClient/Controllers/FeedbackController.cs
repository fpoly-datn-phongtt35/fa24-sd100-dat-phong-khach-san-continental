using Domain.DTO.Feedback;
using Domain.DTO.Paging;
using Domain.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ViewClient.Repositories.IRepository;

namespace ViewClient.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IFeedback _feedbackService;

        public FeedbackController(IFeedback feedbackService)
        {
            _feedbackService = feedbackService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetListFeedbacks(FeedbackGetRequest request)
        {
            var lst = new ResponseData<FeedbackDto>();
            try
            {
                lst = await _feedbackService.GetListFeedbacks(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(lst);
        }

        [HttpPost]
        public async Task<int> SubmitAll(List<FeedbackCreateRequest> lst) 
        {
            try
            {
                foreach (var item in lst) 
                {
                    var rs = await _feedbackService.AddFeedback(item);
                }
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        [HttpPost]
        public async Task<int> UpdateFeedback(FeedbackUpdateRequest request)
        {
            try
            {
                await _feedbackService.UpdateFeedbacnk(request);
                return 1;
            }
            catch
            {
                return -1;
            }
        }
    }
}
