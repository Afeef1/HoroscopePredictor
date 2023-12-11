using System.ComponentModel.DataAnnotations;

namespace HoroscopePredictorAPI.Models
{
    public class RegisterUser
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
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
