using FluentValidation;
using System.Globalization;
using Wallet.Communication.Requests.Transactions.Deposit;
using Wallet.Communication.Utils;
using Wallet.Exceptions;

namespace Wallet.Application.UseCases.Transaction.Deposits
{
    public class CreateDepositValidator : AbstractValidator<RequestCreateDeposit>
    {
        public CreateDepositValidator()
        { 
            RuleFor(t => t.Amount.StringToDecimalCurrency()).TransactionalAmountValidator();
            RuleFor(t => t.Description).MaximumLength(500).WithMessage(ResourceMessageException.TRANSACTIONAL_DESCRIPTION_MAX_LENGTH);
        }
    }
}
