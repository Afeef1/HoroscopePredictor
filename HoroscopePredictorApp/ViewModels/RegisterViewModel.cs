using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HoroscopePredictorApp.ViewModels
{
    public class RegisterViewModel
    {
        [RegularExpression(@"^[a-zA-Z\s']+$", 
            ErrorMessage = "Name can only contain alphabets, space or apostrophe")]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
       
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@])[A-Za-z\d@]{8,}$",
            ErrorMessage="Password must contain atleast one upper case, one lower case, one number, one @ symbol with a length of atleast 8")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password",
            ErrorMessage = "Password and Confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

 
    }
}
