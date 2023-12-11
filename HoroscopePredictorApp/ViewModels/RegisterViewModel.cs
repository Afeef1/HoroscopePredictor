using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HoroscopePredictorApp.ViewModels
{
    public class RegisterViewModel
    {
        [RegularExpression(@"^[a-zA-Z\s']+$", ErrorMessage = "Name can only contain alphabets, space or apostrophe")]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
       
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password",
            ErrorMessage = "Password and Confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

 
    }
}
