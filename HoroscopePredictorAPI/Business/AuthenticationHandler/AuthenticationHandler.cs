using HoroscopePredictorAPI.Data_Access;
using HoroscopePredictorAPI.Data_Access.UserRepository;
using HoroscopePredictorAPI.Helpers;
using HoroscopePredictorAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace HoroscopePredictorAPI.Business.AuthenticationHandler
{
    public class AuthenticationHandler : IAuthenticationHandler
    {

        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;

        public AuthenticationHandler(IConfiguration config, IUserRepository userRepository)
        {

            _config = config;
            _userRepository = userRepository;
        }
        public async Task<RegisterResponseModel> RegisterUser(RegisterUser user)
        {
            var userExists = _userRepository.DoesUserExist(user.Email);

            if (userExists == true)
            {
                return new RegisterResponseModel
                {
                    StatusCode = HttpStatusCode.Conflict,
                    Message = "User with same email already exist"
                };
            }

            string hashedPassword = HashingHelper.GetHashedPassword(user.Password + user.Email);
            user.Password = hashedPassword;
            await _userRepository.AddUser(user);
            return new RegisterResponseModel
            {
                StatusCode = HttpStatusCode.Created,
                Message = "User has been registered successfully"
            };
        }


        public LoginResponseModel LoginUser(LoginUser user)
        {
            string hashedInputPassword = HashingHelper.GetHashedPassword(user.Password + user.Email);
            var currentUser = _userRepository.GetCurrentUser(user, hashedInputPassword);
            if (currentUser == null)
            {
                return new LoginResponseModel
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Message = "User's Email or Password is incorrect"
                };
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config[Constants.JWT__Key]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, currentUser.Name),
                new Claim(ClaimTypes.NameIdentifier, currentUser.Id)
           };


            var token = new JwtSecurityToken(
                issuer: _config[Constants.JWT__Issuer],
                audience: _config[Constants.JWT__Audience],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return new LoginResponseModel
            {
                JwtToken = jwt,
                StatusCode = HttpStatusCode.OK,
                Message = "User loggedin successfully"
            };

        }
    }
}
