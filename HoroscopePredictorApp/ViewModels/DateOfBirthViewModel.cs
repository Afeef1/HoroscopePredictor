using HoroscopePredictorApp.Models;
using System.ComponentModel.DataAnnotations;

namespace HoroscopePredictorApp.ViewModels
{
    public class DateOfBirthViewModel
    {
        [RegularExpression(@"^[a-zA-Z\s']+$",ErrorMessage ="Name can only contain alphabets, space or apostrophe")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Date of Birth Field is Required")]
        public DateTime? DateOfBirth { get; set; }
        [Required(ErrorMessage = "Day Field is Required")]
        public Days? Day { get; set; }
    }
}
