using HoroscopePredictorApp.Data_Access;
using HoroscopePredictorApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HoroscopePredictorApp.Controllers
{
    [Authorize]
    public class HoroscopeController : Controller
    {
      
        private readonly IHoroscopePredictorAPIClient _horoscopeAPI;
        public HoroscopeController(IHoroscopePredictorAPIClient horoscopePredictorAPI)
        {
          
            _horoscopeAPI = horoscopePredictorAPI;
        }
        public IActionResult Index()
        {
            return View();
        }

       
        public IActionResult DateOfBirth()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DateOfBirth(DateOfBirthViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(DetailsByDob), new
                {
                    dateOfBirth = model.DateOfBirth,
                    day = model.Day.ToString().ToLower()
                });
            }
            return View();

        }

        [HttpGet("horoscope/byDob")]
        public async Task<IActionResult> DetailsByDob([FromQuery] DateTime dateOfBirth, [FromQuery] string day)
        {

            var model = await _horoscopeAPI.GetHoroscopeDataFromDateOfBirthAsync(dateOfBirth, day);
            return View("HoroscopePrediction",model);
        }


        public IActionResult Zodiac()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Zodiac(ZodiacViewModel zodiacSign)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(DetailsByZodiac), new
                {
                    zodiac = zodiacSign.Zodiac.ToString(),
                    day = zodiacSign.Day.ToString().ToLower()
                });
            }
            return View();
        }

        [HttpGet("horoscope/zodiac/{zodiac}")]
        public async Task<IActionResult> DetailsByZodiac(string zodiac, [FromQuery] string day)
        {
            var model = await _horoscopeAPI.GetHoroscopeDataFromZodiacAsync(zodiac, day);
            return View("HoroscopePrediction",model);
        }
    }
}
