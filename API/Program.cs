using Domain.Models;
using Domain.Repositories.Repository;
using Domain.Services.IServices;
using Domain.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Domain.Repositories.IRepository;
using Domain.Services.IServices.IAmenity;
using Domain.Services.IServices.IAmenityRoom;
using Domain.Services.IServices.IRoomType;
using Domain.Services.Services.Amenity;
using Domain.Services.Services.AmenityRoom;
using Domain.Services.Services.RoomType;
using Utilities.JWTSettings;
using Utilities.StoredProcedure;
using Domain.Services.IServices.IRoom;
using Domain.Services.IServices.IRoomBooking;
using Domain.Services.IServices.IRoomTypeService;
using Domain.Services.Services.Room;
using Domain.Services.Services.RoomBooking;
using Domain.Services.Services.RoomTypeService;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "abc", builder =>
                {
                    builder.WithOrigins("http://localhost:7114", "https://localhost:7173")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });
            // Add services to the container.
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var privateKey = jwtSettings["PrivateKey"];
            var jwtIssuer = jwtSettings["JWTIssuer"];
            var jwtAudience = jwtSettings["JWTAudience"];
            var expirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"]);

            builder.Services.AddSingleton(new TokenService(privateKey, jwtIssuer, jwtAudience, expirationMinutes));

            builder.Services.AddControllers();

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            

            builder.Services.AddTransient<ServiceTypeRepo>();
            //builder.Services.AddTransient<IServiceTypeRepo, ServiceTypeRepo>();
            builder.Services.AddTransient<IVoucherDetailRepo, VoucherDetailRepo>();
            builder.Services.AddTransient<IVoucherDetailService, VoucherDetailService>();

            builder.Services.AddTransient<IServiceRepo,ServiceRepo>();
            builder.Services.AddTransient<IServiceService, ServiceService>();

            builder.Services.AddTransient<IServiceOrderDetailRepo, ServiceOrderDetailRepo>();
            builder.Services.AddTransient<IServiceOrderDetailService, ServiceOrderDetailService>();

            builder.Services.AddTransient<BuildingRepo>();

            builder.Services.AddTransient<IFloorRepo, FloorRepo>();
            builder.Services.AddTransient<IAmenityRepository, AmenityRepository>();
            builder.Services.AddTransient<IRoomTypeRepository, RoomTypeRepository>();
            builder.Services.AddTransient<IAmenityRoomRepository, AmenityRoomRepository>();
            builder.Services.AddTransient<IRoomRepo, RoomRepo>();
            builder.Services.AddTransient<IRoomTypeServiceRepository, RoomTypeServiceRepository>();
            builder.Services.AddTransient<IRoomBookingRepository, RoomBookingRepository>();
            builder.Services.AddTransient<IRoomBookingDetailRepository, RoomBookingDetailRepository>();
            //room
            builder.Services.AddTransient<IRoomCreateService, RoomCreateService>();
            builder.Services.AddTransient<IRoomDeleteService, RoomDeleteService>();
            builder.Services.AddTransient<IRoomGetService, RoomGetService>();
            builder.Services.AddTransient<IRoomUpdateService, RoomUpdateService>();

            builder.Services.AddTransient<IServiceTypeService, ServiceTypeService>();
            builder.Services.AddTransient<IFloorService, FloorService>();
            builder.Services.AddTransient<IBuildingService, BuildingService>();
            //AmenityService
            builder.Services.AddTransient<IAmenityAddService, AmenityAddService>();
            builder.Services.AddTransient<IAmenityDeleteService, AmenityDeleteService>();
            builder.Services.AddTransient<IAmenityGetService, AmenityGetService>();
            builder.Services.AddTransient<IAmenityUpdateService, AmenityUpdateService>();
            //RoomTypeService
            builder.Services.AddTransient<IRoomTypeAddService, RoomTypeAddService>();
            builder.Services.AddTransient<IRoomTypeDeleteService, RoomTypeDeleteService>();
            builder.Services.AddTransient<IRoomTypeGetService, RoomTypeGetService>();
            builder.Services.AddTransient<IRoomTypeUpdateService, RoomTypeUpdateService>();
            
            builder.Services.AddTransient<ICustomerService, CustomerService>(); 
            //AmenityRoomService
            builder.Services.AddTransient<IAmenityRoomAddService, AmenityRoomAddService>();
            builder.Services.AddTransient<IAmenityRoomDeleteService, AmenityRoomDeleteService>();
            builder.Services.AddTransient<IAmenityRoomGetService, AmenityRoomGetService>();
            builder.Services.AddTransient<IAmenityRoomUpdateService, AmenityRoomUpdateService>();
            //RoomTypeService_Service
            builder.Services.AddTransient<IRoomTypeServiceAddService, RoomTypeServiceAddService>();
            builder.Services.AddTransient<IRoomTypeServiceDeleteService, RoomTypeServiceDeleteService>();
            builder.Services.AddTransient<IRoomTypeServiceGetService, RoomTypeServiceGetService>();
            builder.Services.AddTransient<IRoomTypeServiceUpdateService, RoomTypeServiceUpdateService>();
            //RoomBooking
            builder.Services.AddTransient<IRoomBookingGetService, RoomBookingGetService>();
            builder.Services.AddTransient<IRoomBookingUpdateService, RoomBookingUpdateService>();
            builder.Services.AddTransient<IRoomBookingCreateForCustomerService, RoomBookingCreateForCustomerService>();
            builder.Services.AddTransient<IRoomBookingCreateService, RoomBookingCreateService>();
            //Staff
            builder.Services.AddTransient<IStaffService, StaffService>();

            builder.Services.AddTransient<VoucherRepo>();
			builder.Services.AddTransient<IVoucherService, VoucherService>();

            builder.Services.AddTransient<IPostTypeService, PostTypeService>();
            builder.Services.AddTransient<IPostService, PostService>();

            builder.Services.AddTransient<IRoleService, RoleService>();
            builder.Services.AddTransient<IRoomBookingDetailServiceForCustomer, RoomBookingDetailService>();
			builder.Services.AddDbContext<ContinentalDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("ConnStr"));
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("abc");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

          
            app.MapControllers();

            app.Run();
        }
    }
}
