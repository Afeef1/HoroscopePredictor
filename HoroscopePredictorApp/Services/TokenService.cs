using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HoroscopePredictorApp.Services
{
    public class TokenService:ITokenService
    {
        public readonly IHttpContextAccessor _contextAccessor;
        public TokenService()
        {
            _contextAccessor = new HttpContextAccessor();
        }

        public string? GetAccessToken()
        {
            return _contextAccessor.HttpContext.Request.Cookies["token"];
        }

        public void SetAccessToken(string token)
        {
            _contextAccessor.HttpContext.Response.Cookies.Append("token", token, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.UtcNow.AddDays(1)
            }); 
        }
    }
}
