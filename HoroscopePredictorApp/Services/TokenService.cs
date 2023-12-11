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
            return _contextAccessor.HttpContext.Session.GetString("token");
        }

        public void SetAccessToken(string token)
        {
            _contextAccessor.HttpContext.Session.SetString("token", token); 
        }

        public bool HasAccessToken()
        {
            if (GetAccessToken() != null)
            {
                return true;
            }
            return false;
        }

        public ClaimsPrincipal? GetClaimsPrincipal()
        {
            var accessToken = GetAccessToken();
            if (accessToken == null)
            {
                return null;
            }
            var jwtHandler = new JwtSecurityTokenHandler();
            List<Claim> claims = jwtHandler.ReadJwtToken(accessToken).Claims.ToList();
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return claimsPrincipal;
        }
    }
}
