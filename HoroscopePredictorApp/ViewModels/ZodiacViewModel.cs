using HoroscopePredictorApp.Models;
using System.ComponentModel.DataAnnotations;

namespace HoroscopePredictorApp.ViewModels
{
    public class ZodiacViewModel
    {
        [RegularExpression(@"^[a-zA-Z\s']+$", ErrorMessage = "Name can only contain alphabets, space or apostrophe")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Zodiac Field is Required")]
        public ZodiacSigns? Zodiac { get;set; }

        [Required(ErrorMessage = "Day Field is Required")]
        public Days? Day { get; set; }
    }
}
