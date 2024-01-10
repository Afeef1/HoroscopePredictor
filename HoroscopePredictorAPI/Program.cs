using HoroscopePredictorAPI.APIHandler;
using HoroscopePredictorAPI.Business.AuthenticationHandler;
using HoroscopePredictorAPI.Business.CacheHandler;
using HoroscopePredictorAPI.Business.ExternalHoroscopePrediction;
using HoroscopePredictorAPI.Business.Services;
using HoroscopePredictorAPI.Data_Access.HoroscopeRepository;
using HoroscopePredictorAPI.Data_Access.UserCache;
using HoroscopePredictorAPI.Data_Access.UserRepository;
using HoroscopePredictorAPI.Helpers;
using HoroscopePredictorAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Refit;
using System.Text;

namespace HoroscopePredictorAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
          new OpenApiSecurityScheme()
          {
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearer" }
          },
          new string[] {"Bearer"}
        }
    });
            });


            builder.Services.AddDbContextPool<ApiDbContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString(Constants.HoroscopeDbConnection)));
            builder.Services.AddScoped<IExternalHoroscopePrediction, ExternalHoroscopePrediction>();
            builder.Services.AddScoped<IAuthenticationHandler, AuthenticationHandler>();
            builder.Services.AddScoped<IHoroscopeRepository, HoroscopeRepository>();
            builder.Services.AddScoped<ICacheHandler, CacheHandler>();
            builder.Services.AddScoped<IUserCacheRepository, UserCacheRepository>();
            builder.Services.AddScoped<IUserCacheService, UserCacheService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddRefitClient<IHoroscopePredictorAPIClient>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration[Constants.ExternalAPIConfigurations__APIBaseUrl]));
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = builder.Configuration[Constants.JWT__Audience],
                ValidIssuer = builder.Configuration[Constants.JWT__Issuer],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration[Constants.JWT__Key]))
            };
        });

            var app = builder.Build();
            app.UseExceptionHandler("/error");
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();

        }
    }
}