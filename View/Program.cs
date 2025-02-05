using Domain.Repositories.IRepository;
using Domain.Repositories.Repository;
using Domain.Services.IServices;
using Domain.Services.IServices.IAmenity;
using Domain.Services.IServices.IEditHistory;
using Domain.Services.IServices.IRoom;
using Domain.Services.IServices.IRoomBooking;
using Domain.Services.IServices.IRoomType;
using Domain.Services.IServices.IRoomTypeService;
using Domain.Services.Services;
using Domain.Services.Services.Amenity;
using Domain.Services.Services.EditHistory;
using Domain.Services.Services.Room;
using Domain.Services.Services.RoomBooking;
using Domain.Services.Services.RoomType;
using Domain.Services.Services.RoomTypeService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Net.payOS;

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
            builder.Services.AddTransient<IRoomBookingUpdateService, RoomBookingUpdateService>();
            builder.Services.AddTransient<IFloorService, FloorService>();
            builder.Services.AddTransient<IBuildingRepo, BuildingRepo>();
            builder.Services.AddTransient<IRoomTypeGetService, RoomTypeGetService>();
            builder.Services.AddTransient<IRoomTypeRepository, RoomTypeRepository>();
            builder.Services.AddTransient<IResidenceRegistrationRepo, ResidenceRegistrationRepo>();
            builder.Services.AddTransient<IResidenceRegistrationService, ResidenceRegistrationService>();
            builder.Services.AddTransient<IPaymentHistoryRepository, PaymentHistoryRepository>();
            builder.Services.AddTransient<IPaymentHistoryService, PaymentHistoryService>();
            builder.Services.AddTransient<IEditHistoryRepository, EditHistoryRepository>();
            builder.Services.AddTransient<IEditHistoryAddService, EditHistoryAddService>();
            
            builder.Services.AddSession(option =>
            {
                option.IdleTimeout = TimeSpan.FromSeconds(100);
            });
            builder.Services.AddHttpContextAccessor();

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

            app.UseSession();


            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Statistics}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
