using Domain.Repositories.IRepository;
using Domain.Repositories.Repository;
using Domain.Services.IServices;
using Domain.Services.IServices.IAmenity;
using Domain.Services.IServices.IRoom;
using Domain.Services.IServices.IRoomBooking;
using Domain.Services.IServices.IRoomType;
using Domain.Services.Services;
using Domain.Services.Services.Amenity;
using Domain.Services.Services.Room;
using Domain.Services.Services.RoomBooking;
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
            builder.Services.AddTransient<ICustomerService, CustomerService>();
            builder.Services.AddTransient<IRoomGetService, RoomGetService>();
            builder.Services.AddTransient<IRoomRepo, RoomRepo>();
            builder.Services.AddTransient<IRoomBookingGetService, RoomBookingGetService>();
            builder.Services.AddTransient<IRoomBookingCreateService, RoomBookingCreateService>();
            builder.Services.AddTransient<IRoomBookingRepository, RoomBookingRepository>();
            builder.Services.AddTransient<IRoomBookingDetailServiceForCustomer, RoomBookingDetailService>(); 
            builder.Services.AddTransient<IRoomBookingDetailRepository, RoomBookingDetailRepository>();
            builder.Services.AddTransient<IServiceService, ServiceService>();
            builder.Services.AddTransient<IServiceRepo, ServiceRepo>();
            builder.Services.AddTransient<IServiceTypeService, ServiceTypeService>();
            builder.Services.AddTransient<IServiceTypeRepo, ServiceTypeRepo>();
            builder.Services.AddTransient<IRoomBookingCreateForCustomerService, RoomBookingCreateForCustomerService>();
            builder.Services.AddTransient<IRoomBookingRepository, RoomBookingRepository>(); 
            builder.Services.AddTransient<IRoomUpdateStatusService, RoomUpdateStatusService>(); 
            builder.Services.AddTransient<IServiceOrderDetailService, ServiceOrderDetailService>();
            builder.Services.AddTransient<IServiceOrderDetailRepo, ServiceOrderDetailRepo>();

            builder.Services.AddHttpClient();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Authorization/LoginForm");
                options.LoginPath = new PathString("/Authorization/LoginForm");
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
