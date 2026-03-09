using FluentValidation;
using System.Globalization;
using Wallet.Communication.Requests.Deposit;
using Wallet.Exceptions;

namespace Wallet.Application.UseCases.Transaction.Deposits
{
    public class CreateDepositValidator : AbstractValidator<RequestCreateDeposit>
    {
        public CreateDepositValidator()
        { 
            RuleFor(t => decimal.Parse(t.Amount, CultureInfo.InvariantCulture)).TransactionalAmountValidator();
            RuleFor(t => t.Description).MaximumLength(500).WithMessage(ResourceMessageException.TRANSACTIONAL_DESCRIPTION_MAX_LENGTH);
        }
    }
}
