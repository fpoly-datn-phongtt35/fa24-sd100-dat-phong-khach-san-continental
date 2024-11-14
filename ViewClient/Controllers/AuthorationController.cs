using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using ViewClient.Models.DTO.Login;
using ViewClient.Repositories.IRepository;
using ViewClient.Repositories.Repository;

namespace ViewClient.Controllers
{
    public class AuthorationController : Controller
    {
        private readonly ILogin _loginRepo;
        public AuthorationController(ILogin loginRepo)
        {
            _loginRepo = loginRepo;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginInputRequest loginInput)
        {
            if (ModelState.IsValid)
            {
                var result = await _loginRepo.LoginAsync(loginInput);
                if (result != null)
                {
                    HttpContext.Session.SetString("UserName", result.UserName);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Tài khoản hoặc mật khẩu không đúng.");
                }
            }

            return View(loginInput);
        }
    }
}
