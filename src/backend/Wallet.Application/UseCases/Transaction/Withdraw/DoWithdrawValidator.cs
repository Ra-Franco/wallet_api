using FluentValidation;
using Wallet.Communication.Requests.Transactions.Withdraw;
using Wallet.Communication.Utils;
using Wallet.Exceptions;

namespace Wallet.Application.UseCases.Transaction.Withdraw
{
    public class DoWithdrawValidator : AbstractValidator<RequestCreateWithdraw>
    {
        public DoWithdrawValidator()
        {
            RuleFor(req => req.Amount.StringToDecimalCurrency()).TransactionalAmountValidator();
            RuleFor(req => req.TransactionPassword).TransactionPasswordIsValid();
            RuleFor(req => req.Description).MaximumLength(500).WithMessage(ResourceMessageException.TRANSACTIONAL_DESCRIPTION_MAX_LENGTH);
        }
    }
}
