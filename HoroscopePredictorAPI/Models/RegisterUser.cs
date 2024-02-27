using System.ComponentModel.DataAnnotations;

namespace HoroscopePredictorAPI.Models
{
    public class RegisterUser
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
      
        public string Name { get; set; }

        public string Email { get; set; }
       
        [DataType(DataType.Password)]
        public string Password { get; set; }

       

    }
}
