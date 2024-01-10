using HoroscopePredictorAPI.APIHandler;
using HoroscopePredictorAPI.Business;
using HoroscopePredictorAPI.Business.ExternalHoroscopePrediction;
using HoroscopePredictorAPI.Business.Services;
using HoroscopePredictorAPI.Helpers;
using HoroscopePredictorAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace HoroscopePredictorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ResponseCache(Duration =Constants.MaxAge)]
    public class HoroscopeController : ControllerBase
    {
        private readonly IExternalHoroscopePrediction _externalHoroscopePrediction;
        private readonly IUserCacheService _userCacheService;

        public HoroscopeController(IExternalHoroscopePrediction externalHoroscopePrediction,IUserCacheService userCacheService)
        {
            _externalHoroscopePrediction = externalHoroscopePrediction;
            _userCacheService = userCacheService;
        }

        [Authorize]
        [HttpGet("{zodiac}")]
        public async Task<IActionResult> GetHoroscopeByZodiac(string zodiac, string day)
        {
            var horoscopeData = await _externalHoroscopePrediction.FetchDataFromAPIUsingZodiacSign(zodiac, day);
            List<Claim> claim = User.Claims.ToList();
            var userId = claim.Find(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            _userCacheService.AddUserHistoryData(zodiac, userId);
            return Ok(horoscopeData);
        }

        [Authorize]
        [HttpGet("dob")]
        public async Task<IActionResult> GetHoroscopeByDateOfBirth(DateTime dateOfBirth, string day)
        {
            var zodiac = ZodiacSignCalculator.GetZodiacSignFromDateOfBirth(dateOfBirth);
            var horoscopeData = await _externalHoroscopePrediction.FetchDataFromAPIUsingZodiacSign(zodiac, day);
            List<Claim> claim = User.Claims.ToList();
            var userId = claim.Find(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            _userCacheService.AddUserHistoryData(zodiac, userId);
            return Ok(horoscopeData);
        }


        


    }
}
