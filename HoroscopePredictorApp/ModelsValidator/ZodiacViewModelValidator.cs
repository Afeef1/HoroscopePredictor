using FluentValidation;
using HoroscopePredictorApp.ViewModels;

namespace HoroscopePredictorApp.ModelsValidator
{
    public class ZodiacViewModelValidator : AbstractValidator<ZodiacViewModel>
    {
        public ZodiacViewModelValidator()
        {
            RuleFor(u => u.Zodiac).NotNull().NotEmpty().WithMessage("Zodiac Field is Required");
            RuleFor(u => u.Day).NotNull().NotEmpty().WithMessage("Day Field is Required");

        }
    }
}
