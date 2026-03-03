using FluentValidation;
using Wallet.Communication.Requests;
using Wallet.Communication.Requests.Wallet;

namespace Wallet.Application.UseCases.Wallet.TransactionalPassword
{
    public class SetTransactionalPasswordValidator : AbstractValidator<RequestSetTransactionPasswordJson>
    {
        public SetTransactionalPasswordValidator()
        {
            RuleFor(req => req.TransactionPassword).TransactionPasswordIsValid();
        }
    }
}
