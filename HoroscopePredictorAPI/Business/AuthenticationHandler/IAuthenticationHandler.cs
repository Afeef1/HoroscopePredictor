using HoroscopePredictorAPI.Models;

namespace HoroscopePredictorAPI.Business.AuthenticationHandler
{
    public interface IAuthenticationHandler 
    {
        public Task<RegisterResponseModel> RegisterUser(RegisterUser user);

        public LoginResponseModel LoginUser(LoginUser loginUser);

    }
}
