using HoroscopePredictorApp.Data_Access;
using HoroscopePredictorApp.Models;
using HoroscopePredictorApp.Services;
using HoroscopePredictorApp.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Refit;

namespace HoroscopePredictorApp.Controllers
{
    public class UserController : Controller
    {
        private IHoroscopePredictorAPIClient _horoscopePredictorAPIClient;
        public ITokenService _tokenService;

        public UserController(IHoroscopePredictorAPIClient horoscopePredictorAPIClient, ITokenService tokenService)
        {
            _horoscopePredictorAPIClient = horoscopePredictorAPIClient;
            _tokenService = tokenService;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {
                return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new RegisterUser
                    {
                        Name = model.Name,
                        Email = model.Email,
                        Password = model.Password,
                    };
                    var result = await _horoscopePredictorAPIClient.Register(user);
                    if(result.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                    return RedirectToAction(nameof(Login), nameof(User));

                    }

                }
            }
            catch (ApiException ex)
            {
                RegisterUserResponseModel? error = await ex.GetContentAsAsync<RegisterUserResponseModel>();

                ViewBag.HasLoginFailed = true;
                ViewBag.LoginErrorMessage = error?.Message;
                return View();

            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUser model,string? returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    LoginUserResponseModel result = await _horoscopePredictorAPIClient.Login(model);
                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        _tokenService.SetAccessToken(result.JwtToken);
                        var claimsPrincipal = _tokenService.GetClaimsPrincipal();
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties
                        {
                            IsPersistent = false
                        });
                        if(!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        return RedirectToAction(nameof(Index), "Home");
                    }

                }

            }
            catch (ApiException ex)
            {
                LoginUserResponseModel? error = await ex.GetContentAsAsync<LoginUserResponseModel>();

                ViewBag.HasLoginFailed = true;
                ViewBag.LoginErrorMessage = error?.Message;
                return View();


            }
            return View();
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet]
        public async Task<IActionResult> History()
        {
            var zodiacHistory = await _horoscopePredictorAPIClient.History();
            return View(zodiacHistory);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
