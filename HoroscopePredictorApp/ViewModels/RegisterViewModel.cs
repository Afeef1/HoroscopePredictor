using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HoroscopePredictorApp.ViewModels
{
    public class RegisterViewModel
    {

        public string Name { get; set; }

       
        public string Email { get; set; }

        
        [DataType(DataType.Password)]

        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }

 
    }
}
