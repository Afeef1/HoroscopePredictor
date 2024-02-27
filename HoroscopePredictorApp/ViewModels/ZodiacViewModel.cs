using HoroscopePredictorApp.Models;
using System.ComponentModel.DataAnnotations;

namespace HoroscopePredictorApp.ViewModels
{
    public class ZodiacViewModel
    {
        public ZodiacSigns? Zodiac { get;set; }

        public Days? Day { get; set; }
    }
}
