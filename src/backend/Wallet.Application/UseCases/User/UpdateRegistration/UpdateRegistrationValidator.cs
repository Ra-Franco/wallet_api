using FluentValidation;
using Wallet.Communication.Requests.User;
using Wallet.Exceptions;

namespace Wallet.Application.UseCases.User.UpdateRegistration
{
    public class UpdateRegistrationValidator : AbstractValidator<RequestUpdateRegistrationUser>
    {
        public UpdateRegistrationValidator()
        {
            RuleFor(req => req.Email).NotEmpty().WithMessage(ResourceMessageException.EMAIL_EMPTY);
            When(req => !string.IsNullOrEmpty(req.Email), () =>
            {
                RuleFor(req => req.Email).EmailAddress().WithMessage(ResourceMessageException.EMAIL_INVALID);
            });
            RuleFor(req => req.Occupation).NotEmpty().WithMessage(ResourceMessageException.OCCUPATION_EMPTY);
            RuleFor(req => req.Income).NotEmpty().WithMessage(ResourceMessageException.INCOME_EMPTY);
            RuleFor(req => req.Phonenumber).BrazilPhoneNumberValidator();
        }
    }
}
