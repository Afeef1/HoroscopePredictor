using HoroscopePredictorApp.Models;
using System.ComponentModel.DataAnnotations;

namespace HoroscopePredictorApp.ViewModels
{
    public class ZodiacViewModel
    {
        [Required(ErrorMessage ="Zodiac Field is Required")]
        public ZodiacSigns? Zodiac { get;set; }

        [Required(ErrorMessage = "Day Field is Required")]
        public Days? Day { get; set; }
    }
}
