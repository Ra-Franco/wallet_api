using Wallet.Communication.Requests.Transactions.Transfer;
using Wallet.Communication.Responses.Transaction;

namespace Wallet.Application.UseCases.Transaction.Transfer
{
    public interface IDoTransferUseCase
    {
        public Task<ResponseTransfer> Execute(RequestTransfer request);
    }
}
