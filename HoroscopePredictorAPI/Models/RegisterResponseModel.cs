using System.Net;

namespace HoroscopePredictorAPI.Models
{
    public class RegisterResponseModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message {  get; set; }

    }
}
