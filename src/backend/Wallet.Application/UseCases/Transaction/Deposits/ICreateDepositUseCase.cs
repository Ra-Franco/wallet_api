using Wallet.Communication.Requests.Transactions.Deposit;
using Wallet.Communication.Responses.Transaction;

namespace Wallet.Application.UseCases.Transaction.Deposits
{
    public interface ICreateDepositUseCase
    {
        public Task<ResponseShortTransaction> Execute(RequestCreateDeposit request);
    }
}
