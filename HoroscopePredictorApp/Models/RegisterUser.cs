using System.ComponentModel.DataAnnotations;

namespace HoroscopePredictorApp.Models
{
    public class RegisterUser
    {

        public string Name { get; set; }

        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
