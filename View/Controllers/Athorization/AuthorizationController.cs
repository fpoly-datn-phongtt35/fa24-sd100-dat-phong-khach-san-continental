using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Domain.Services.IServices;
using Domain.DTO.Athorization;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace View.Controllers.Athorization
{
    public class AuthorizationController : Controller
    {
        private readonly IStaffService _staffService;
        public AuthorizationController(IStaffService staffService)
        {
            _staffService = staffService;
        }
        public IActionResult LoginForm()
        {
            return View();
        }

        public async Task<IActionResult> Login(LoginSubmitModel model)
        {
            try 
            {
                var obj = await _staffService.Login(model);
                if (obj != null && obj.Id != Guid.Empty)
                {
                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, obj.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Email, obj.Email));
                    claims.Add(new Claim("RoleId", obj.RoleId.ToString()));
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
                        IsPersistent = true
                    };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                    return Ok(new 
                    {
                        Success = true,
                        Message = "Đăng nhập thành công!"
                    });
                }
                return Ok(new
                {
                    Success = false,
                    Message = "Đăng nhập thất bại!"
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> GetCurrentUserClaims()
        {
            try 
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var roleId = User.FindFirstValue("RoleId");

                // Check if all claims are found (optional)
                if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(userEmail) && !string.IsNullOrEmpty(roleId))
                {
                    return Ok(new { Id = userId, Email = userEmail, RoleId = roleId });
                }
                return Ok(null);
            }
             catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
