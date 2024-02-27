using FluentValidation;
using HoroscopePredictorApp.Models;

namespace HoroscopePredictorApp.ModelsValidator
{
    public class LoginUserValidator : AbstractValidator<LoginUser>
    {
        public LoginUserValidator()
        {
            RuleFor(x => x.Email).NotNull().EmailAddress();
            RuleFor(x => x.Password).NotNull();
        }

    }
}
