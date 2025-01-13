using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.FileProviders;
using ViewClient.Repositories.IRepository;
using ViewClient.Repositories.Repository;

namespace ViewClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
          
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpClient<ILogin, Login>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7130");
            });
            builder.Services.AddTransient<ILogin, Login>();
            builder.Services.AddTransient<IRegister, Register>();
            builder.Services.AddTransient<IRoom, Room>();
            builder.Services.AddTransient<ICustomer, Customer>();
            builder.Services.AddTransient<IRoomBookingDetail, RoomBookingDetail>();
            builder.Services.AddTransient<IRoombooking, RoomBooking>();
            builder.Services.AddTransient<IServiceOderDetail, ServiceOrderDetail>();
            builder.Services.AddTransient<ISendEmail, SendEmail>();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Authoration/Login");
                options.LoginPath = new PathString("/Authoration/Login");
                options.ReturnUrlParameter = "url";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.SlidingExpiration = true;

                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = "Net.Security.Cookie",
                    Path = "/",
                    SameSite = SameSiteMode.Lax,
                    SecurePolicy = CookieSecurePolicy.SameAsRequest
                };

            });
            builder.Services.AddMvc().AddSessionStateTempDataProvider();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), @"..\View\wwwroot\images")),
                RequestPath = "/View/wwwroot/images"
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "login",
                pattern: "login",
                defaults: new { controller = "Authoration", action = "Login" });
            app.MapControllerRoute(
                 name: "register",
                 pattern: "register",
                 defaults: new { controller = "Authoration", action = "Register" });
            app.MapControllerRoute(
                name: "roombooking",
                pattern: "roombooking",
                defaults: new { controller = "RoomBooking", action = "RoomBooking" });
            app.MapControllerRoute(
               name: "bookingHistory",
               pattern: "bookingHistory",
               defaults: new { controller = "RoomBooking", action = "BookingHistory" });
            app.Run();
        }
    }
}
