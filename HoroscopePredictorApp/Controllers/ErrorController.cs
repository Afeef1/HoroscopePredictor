using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Refit;

namespace HoroscopePredictorApp.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {

            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Not Found";
                    break;

            }
            return View("NotFound");
        }

        [Route("Error")]
        public async Task<IActionResult> ErrorAsync()
        {
            var error = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (error?.Error is ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await HttpContext.SignOutAsync();
                    return RedirectToAction(nameof(UserController.Login), "User");
                }
            }

            return View("Error");
        }
    }
}
