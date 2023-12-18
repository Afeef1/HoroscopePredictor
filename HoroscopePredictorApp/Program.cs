using HoroscopePredictorApp.Data_Access;
using HoroscopePredictorApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace HoroscopePredictorApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSession();
            builder.Services.AddControllersWithViews();
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
            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}");

            app.Run();
            
        }
    }
}