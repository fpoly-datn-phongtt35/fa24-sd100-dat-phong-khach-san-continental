using Domain.DTO.Feedback;
using Domain.DTO.Paging;
using Domain.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ViewClient.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
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
                lst = await _feedbackService.GetListFeedback(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(lst);
        }
    }
}
