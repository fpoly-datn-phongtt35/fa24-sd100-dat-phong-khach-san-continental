using Domain.Repositories.IRepository;
using Domain.Repositories.Repository;
using Domain.Services.IServices;
using Domain.Services.IServices.IAmenity;
using Domain.Services.IServices.IRoomType;
using Domain.Services.Services;
using Domain.Services.Services.Amenity;
using Domain.Services.Services.RoomType;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace View
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddTransient<IStaffService,StaffService>();

            builder.Services.AddHttpClient();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Authorization/LoginForm");
                options.LoginPath = new PathString("/Authorization/LoginForm");
                options.ReturnUrlParameter = "url";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // n?u d�ng ExpireTimeSpan th�  SlidingExpiration ph?i set l� false. Nh? v?y cho d� t??ng t�c hay k t??ng t�c th� ??u timeout theo th?i gian ?� set
                options.SlidingExpiration = true; //???c s? d?ng ?? thi?t l?p th?i gian s?ng c?a cookie d?a tr�n th?i gian cu?i c�ng m� ng??i d�ng ?� t??ng t�c v?i ?ng d?ng . N?u ng??i d�ng ti?p t?c t??ng t�c v?i ?ng d?ng tr??c khi cookie h?t h?n, th?i gian s?ng c?a cookie s? ???c gia h?n th�m.

                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = "Net.Security.Cookie",
                    Path = "/",
                    SameSite = SameSiteMode.Lax,
                    SecurePolicy = CookieSecurePolicy.SameAsRequest
                };

            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
