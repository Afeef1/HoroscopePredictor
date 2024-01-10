using HoroscopePredictorApp.Models;
using System.ComponentModel.DataAnnotations;

namespace HoroscopePredictorApp.ViewModels
{
    public class DateOfBirthViewModel
    {
        [Required(ErrorMessage ="Date of Birth Field is Required")]
        public DateTime? DateOfBirth { get; set; }
        [Required(ErrorMessage = "Day Field is Required")]
        public Days? Day { get; set; }
    }
}
