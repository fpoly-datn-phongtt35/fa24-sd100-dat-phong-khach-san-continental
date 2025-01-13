using Domain.DTO.Email;
using Microsoft.AspNetCore.Mvc;
using ViewClient.Repositories.IRepository;

namespace ViewClient.Controllers
{
    public class EmailController : Controller
    {
        private readonly ISendEmail _Repo;
        private readonly HttpClient _httpClient;
        public EmailController(ISendEmail Repo, HttpClient httpClient)
        {
            _Repo = Repo;
            _httpClient = httpClient;
        }
        [HttpPost("sendemail")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendEmailAsync(EmailRequest request)
        {
            var result = await _Repo.SendEmailAsync(request);
            return View();
        }
        [HttpPost("sendaccount")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendAccountAsync(AccountRequest request)
        {
            var result = await _Repo.SendAccountAsync(request);
            return View();
        }
    }
}
