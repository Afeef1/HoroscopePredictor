using System.Security.Claims;

namespace HoroscopePredictorApp.Services
{
    public interface ITokenService
    {
        public string GetAccessToken();
        public void SetAccessToken(string token);

        public bool HasAccessToken();

        ClaimsPrincipal? GetClaimsPrincipal();
    }
}
