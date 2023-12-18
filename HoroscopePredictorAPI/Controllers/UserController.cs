using HoroscopePredictorAPI.Helpers;
using HoroscopePredictorAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using HoroscopePredictorAPI.Business;
using HoroscopePredictorAPI.Business.Services;

namespace HoroscopePredictorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Business.AuthenticationHandler.IAuthenticationHandler _authenticationHandler;
        private readonly IUserCacheService _userCacheService;

        public UserController(Business.AuthenticationHandler.IAuthenticationHandler authenticationHandler, IUserCacheService userCacheService)
        {
            _authenticationHandler = authenticationHandler;
            _userCacheService = userCacheService;
        }

        [HttpPost("[action]")]
      
        public async Task<IActionResult> Register(RegisterUser user)
        {
            var RegisterUserResponse = await _authenticationHandler.RegisterUser(user);
            if (RegisterUserResponse.StatusCode == HttpStatusCode.Conflict)
            {
                return Conflict(RegisterUserResponse);
            }

            return StatusCode(StatusCodes.Status201Created, RegisterUserResponse);
            
        }

         [HttpPost("[action]")]
        
        public IActionResult Login(LoginUser user)
        {
            var LoginUserResponse = _authenticationHandler.LoginUser(user);
            if (LoginUserResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Unauthorized(LoginUserResponse);
            }

            return Ok(LoginUserResponse);
        }

        [Authorize]
        [HttpGet("History")]
        public IActionResult UserSearchHistory()
        {
            List<Claim> claim = User.Claims.ToList();
            var userId = claim.Find(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            List<string> zodiacSearchHistory = _userCacheService.GetUserHistoryData(userId);
            return Ok(zodiacSearchHistory);
        }

       
    }





    }

