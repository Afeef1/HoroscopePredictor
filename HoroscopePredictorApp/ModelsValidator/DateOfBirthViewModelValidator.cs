using FluentValidation;
using HoroscopePredictorApp.ViewModels;

namespace HoroscopePredictorApp.ModelsValidator
{
    public class DateOfBirthViewModelValidator : AbstractValidator<DateOfBirthViewModel>
    {
        public DateOfBirthViewModelValidator()
        {
            RuleFor(u=>u.DateOfBirth).NotNull().NotEmpty().WithMessage("Date of Birth Field is Required");
            RuleFor(u => u.Day).NotNull().NotEmpty().WithMessage("Day Field is Required");
        }
    }
}
