using HoroscopePredictorApp.Data_Access;
using HoroscopePredictorApp.Models;
using HoroscopePredictorApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HoroscopePredictorApp.Controllers
{
    public class HomeController : Controller
    {


        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

    }
}