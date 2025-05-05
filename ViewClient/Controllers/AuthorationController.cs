using Domain.Enums;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Claims;
using ViewClient.Models.DTO.Login;
using ViewClient.Models.DTO.Register;
using ViewClient.Repositories.IRepository;
using ViewClient.Repositories.Repository;

namespace ViewClient.Controllers
{
    public class AuthorationController : Controller
    {
        private readonly ILogin _loginRepo;
        private readonly IRegister _registerRepo;
        private readonly ISendEmail _sendEmail;
        private readonly ICustomer _customerRepo;
        public AuthorationController(ILogin loginRepo, IRegister registerRepo, ICustomer customerRepo)
        {
            _loginRepo = loginRepo;
            _registerRepo = registerRepo;
            _customerRepo = customerRepo;
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

                // Check if result is null or the status is not Active
                if (result == null || result.Status != EntityStatus.Active)
                {
                    ModelState.AddModelError(string.Empty, "Tài khoản hoặc mật khẩu không đúng.");
                    return View(loginInput);
                }
                var checkIsCustomerRestricted = await _customerRepo.IsCustomerRestrictedAsync(result.Id);
                if(checkIsCustomerRestricted == true)
                {
                    ModelState.AddModelError(string.Empty, "Tài khoản đã bị khóa. Vui lòng liên hệ CSKH để được hỗ trợ.");
                    return View(loginInput);
                }
                // Proceed with successful login
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.UserData, result.Id.ToString()),
            new Claim(ClaimTypes.Name, result.UserName)
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "Home");
            }

            // If we got this far, something failed, redisplay form
            return View(loginInput);
        }
        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterInputRequest registerInput)
        {
            if (ModelState.IsValid)
            {
                var result = await _registerRepo.RegisterAsync(registerInput);
                if (result != null)
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.UserData, result.Id.ToString()),
                new Claim(ClaimTypes.Name, result.UserName)
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
                        IsPersistent = true
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties
                    );

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Thông tin đã tồn tại. Hãy nhập thông tin cá nhân của bạn. " +
                        "Hãy thử thay đổi tài khoản!");
                }
            }

            // Return the registration view with the current model to display validation errors
            return View(registerInput);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
