using HoroscopePredictorApp.Models;
using System.ComponentModel.DataAnnotations;

namespace HoroscopePredictorApp.ViewModels
{
    public class DateOfBirthViewModel
    {
        public DateTime? DateOfBirth { get; set; }
        public Days? Day { get; set; }
    }
}
