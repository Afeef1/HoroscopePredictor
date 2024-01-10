using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HoroscopePredictorAPI.Controllers
{
   
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet("error")]
        public IActionResult HandleError()
        {
            return StatusCode(StatusCodes.Status500InternalServerError,"Something Went Wrong");
        }

    }
}
