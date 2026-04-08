using FluentValidation;
using System.Text.RegularExpressions;
using Wallet.Communication.Requests.User;
using Wallet.Exceptions;

namespace Wallet.Application.UseCases.User.Register
{
    public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
    {
        public RegisterUserValidator() 
        {
            RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceMessageException.NAME_EMPTY);
            RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceMessageException.EMAIL_EMPTY);
            When(user => !string.IsNullOrEmpty(user.Email), () =>
            {
                RuleFor(user => user.Email).EmailAddress().WithMessage(ResourceMessageException.EMAIL_INVALID);
            });
            RuleFor(user => user.Password.Length).GreaterThanOrEqualTo(6).WithMessage(ResourceMessageException.PASSWORD_INVALID);
            RuleFor(user => user.CPF)
                .NotEmpty().WithMessage(ResourceMessageException.CPF_EMPTY)
                .CpfIsValid();
            RuleFor(user => user.Occupation).NotEmpty().WithMessage(ResourceMessageException.OCCUPATION_EMPTY);
            RuleFor(user => user.Income).NotEmpty().WithMessage(ResourceMessageException.INCOME_EMPTY);
            RuleFor(user => user.Gender)
                .Must(g => g == "F" || g == "M")
                .WithMessage(ResourceMessageException.GENDER_INVALID);
        }
    }
}
