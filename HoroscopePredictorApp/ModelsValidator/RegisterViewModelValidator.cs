using FluentValidation;
using HoroscopePredictorApp.ViewModels;

namespace HoroscopePredictorApp.ModelsValidator
{
    public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterViewModelValidator()
        {
            RuleFor(u => u.Name).NotNull().NotEmpty().Matches(@"^[a-zA-Z\s']+$")
              .WithMessage("Name can only contain alphabets, space or apostrophe");
            RuleFor(u => u.Email).NotNull().NotEmpty().EmailAddress();
            RuleFor(u => u.Password).NotNull().NotEmpty().Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@])[A-Za-z\d@]{8,}$")
                .WithMessage("Password must contain atleast one upper case, one lower case, one number, one @ symbol with a length of atleast 8");
            RuleFor(u => u.ConfirmPassword).Equal(u => u.Password).WithMessage("Password and Confirmation password do not match.");
        }
    }
}
