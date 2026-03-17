using FluentValidation;
using Wallet.Communication.Requests.Transactions.Transfer;
using Wallet.Exceptions;

namespace Wallet.Application.UseCases.Transaction.Transfer
{
    public class DoTransferValidator : AbstractValidator<RequestTransfer>
    {
        public DoTransferValidator()
        {
            RuleFor(t => t.TransactionPassword).TransactionPasswordIsValid();
            RuleFor(t => t.ReceiverCpf).CpfIsValid();
            RuleFor(t => t.Amount).TransactionalAmountValidator();
            RuleFor(t => t.Description).MaximumLength(500).WithMessage(ResourceMessageException.TRANSACTIONAL_DESCRIPTION_MAX_LENGTH);
        }
    }
}
