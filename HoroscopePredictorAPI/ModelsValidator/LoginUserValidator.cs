using FluentValidation;
using HoroscopePredictorAPI.Models;

namespace HoroscopePredictorAPI.ModelsValidator
{
    public class LoginUserValidator : AbstractValidator<LoginUser>
    {
        public LoginUserValidator()
        {
            RuleFor(u => u.Email).NotNull().NotEmpty().EmailAddress();
            RuleFor(u => u.Password).NotNull().NotEmpty();
        }
    }
}
