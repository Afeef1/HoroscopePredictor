using System.ComponentModel.DataAnnotations;

namespace HoroscopePredictorApp.Models
{
    public class RegisterUser
    {
       
      
        [Required] 
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
