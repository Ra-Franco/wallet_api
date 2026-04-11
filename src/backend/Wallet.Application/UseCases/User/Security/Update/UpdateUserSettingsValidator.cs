using FluentValidation;
using Wallet.Communication.Requests.User.Security;
using Wallet.Communication.Utils;
using Wallet.Exceptions;

namespace Wallet.Application.UseCases.User.Security.Update
{
    public class UpdateUserSettingsValidator : AbstractValidator<RequestUpdateSecuritySettings>
    {
        public UpdateUserSettingsValidator()
        {
            RuleFor(req => req.TransactionLimitPeriod).IsInEnum().WithMessage(ResourceMessageException.TRANSACTION_LIMIT_PERIOD_INVALID);
            RuleFor(req => req.TransactionLimit.StringToDecimalCurrency()).GreaterThan(-1).WithMessage(ResourceMessageException.TRANSACTION_LIMIT_NOT_NEGATIVE);
        }
    }
}
