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
        public async Task<int> Submit(FeedbackCreateRequest request) 
        {
            try
            {
                var rs = await _feedbackService.AddFeedback(request);
                return rs;
            }
            catch
            {
                return -1;
            }
        }
    }
}
