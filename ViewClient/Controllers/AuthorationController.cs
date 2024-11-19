﻿using Domain.Enums;
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
        public AuthorationController(ILogin loginRepo, IRegister registerRepo)
        {
            _loginRepo = loginRepo;
            _registerRepo = registerRepo;
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
                if (result != null && result.Status == EntityStatus.Active)
                {
                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.UserData, result.Id.ToString()));
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
                        IsPersistent = true
                    };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
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
                    HttpContext.Session.SetString("UserName", result.UserName);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Yêu cầu nhập đúng thông tin.");
                }
            }

            // Return the registration view with the current model to display validation errors
            return View(registerInput);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserName");
            return RedirectToAction("Index", "Home");
        }
    }
}
