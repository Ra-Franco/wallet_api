using Wallet.Communication.Requests.Transactions.Withdraw;
using Wallet.Communication.Responses.Transaction;

namespace Wallet.Application.UseCases.Transaction.Withdraw
{
    public interface IDoWithdrawUseCase
    {
        public Task<ResponseTransaction> Execute(RequestCreateWithdraw request);
    }
}
