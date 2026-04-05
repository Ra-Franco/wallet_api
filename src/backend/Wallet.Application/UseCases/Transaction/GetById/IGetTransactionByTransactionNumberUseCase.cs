using Wallet.Communication.Responses.Transaction;

namespace Wallet.Application.UseCases.Transaction.GetById
{
    public interface IGetTransactionByTransactionNumberUseCase
    {
        public Task<ResponseTransaction> Execute(string transactionNumber);
    }
}
