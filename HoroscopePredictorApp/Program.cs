using FluentValidation;
using FluentValidation.AspNetCore;
using HoroscopePredictorApp.Data_Access;
using HoroscopePredictorApp.Models;
using HoroscopePredictorApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Refit;
using System;
using System.Reflection;

namespace HoroscopePredictorApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews()
                .AddFluentValidation(c =>
                {
                    c.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                    c.AutomaticValidationEnabled = true;
                    c.ConfigureClientsideValidation(enabled: true);
                });
            builder.Services.AddRefitClient<IHoroscopePredictorAPIClient>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["HoroscopeAppConfiguration:BaseUrl"]))
                .AddHttpMessageHandler<HttpHeaderHandler>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddTransient<HttpHeaderHandler>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/user/login";
                });

            var app = builder.Build();


            if (!app.Environment.IsDevelopment())
            {

                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                app.UseExceptionHandler("/Error");

            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}");

            app.Run();

        }
    }
}