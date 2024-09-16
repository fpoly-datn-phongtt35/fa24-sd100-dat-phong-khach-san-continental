using Domain.Models;
using Domain.Repositories.Repository;
using Domain.Services.IServices;
using Domain.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Utilities.JWTSettings;

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
            builder.Services.AddTransient<ServiceRepo>();
            builder.Services.AddTransient<BuildingRepo>();

            builder.Services.AddTransient<IServiceTypeService, ServiceTypeService>();
            builder.Services.AddTransient<IServiceService, ServiceService>();
            builder.Services.AddTransient<IBuildingService, BuildingService>();
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
