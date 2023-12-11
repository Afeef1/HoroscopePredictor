
using System.Net;

namespace HoroscopePredictorApp.Models
{
    public class RegisterUserResponseModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }

    }
}
