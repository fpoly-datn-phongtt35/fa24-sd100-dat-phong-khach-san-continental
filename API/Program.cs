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
using Domain.Services.IServices.RoomType;
using Domain.Services.Services.Amenity;
using Domain.Services.Services.RoomType;
using Utilities.JWTSettings;
using Utilities.StoredProcedure;

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
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           ;
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
            builder.Services.AddTransient<IServiceRepo,ServiceRepo>();
            builder.Services.AddTransient<IServiceOrderRepo, ServiceOrderRepo>();
            builder.Services.AddTransient<BuildingRepo>();

            builder.Services.AddTransient<IFloorRepo, FloorRepo>();
            builder.Services.AddTransient<IAmenityRepository, AmenityRepository>();
            builder.Services.AddTransient<IRoomTypeRepository, RoomTypeRepository>();
            
            builder.Services.AddTransient<IServiceTypeService, ServiceTypeService>();
            builder.Services.AddTransient<IServiceService, ServiceService>();
            builder.Services.AddTransient<IServiceOrderService, ServiceOrderService>();
            builder.Services.AddTransient<IFloorService, FloorService>();
            builder.Services.AddTransient<IBuildingService, BuildingService>();
            //AmenityService
            builder.Services.AddTransient<IAmenityAddService, AmenityAddService>();
            builder.Services.AddTransient<IAmenityDeleteService, AmenityDeleteService>();
            builder.Services.AddTransient<IAmenityGetService, AmenityGetService>();
            builder.Services.AddTransient<IAmenityRollBackService, AmenityRollBackService>();
            builder.Services.AddTransient<IAmenityUpdateService, AmenityUpdateService>();
            //RoomTypeService
            builder.Services.AddTransient<IRoomTypeAddService, RoomTypeAddService>();
            builder.Services.AddTransient<IRoomTypeDeleteService, RoomTypeDeleteService>();
            builder.Services.AddTransient<IRoomTypeGetService, RoomTypeGetService>(); 
            builder.Services.AddTransient<IRoomTypeRollBackService, RoomTypeRollBackService>();
            builder.Services.AddTransient<IRoomTypeUpdateService, RoomTypeUpdateService>();
            
            
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

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("abc");
            app.MapControllers();

            app.Run();
        }
    }
}
