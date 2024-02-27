using FluentValidation;
using HoroscopePredictorAPI.Models;

namespace HoroscopePredictorAPI.ModelsValidator
{
    public class RegisterUserValidator : AbstractValidator<RegisterUser>
    {
        public RegisterUserValidator()
        {
            RuleFor(u => u.Name).NotNull().NotEmpty().Matches(@"^[a-zA-Z\s']+$")
                .WithMessage("Name can only contain alphabets, space or apostrophe");
            RuleFor(u => u.Email).NotNull().NotEmpty().EmailAddress();
            RuleFor(u => u.Password).NotNull().NotEmpty().Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@])[A-Za-z\d@]{8,}$")
                .WithMessage("Password must contain atleast one upper case, one lower case, one number, one @ symbol with a length of atleast 8");
        }
    }
}
