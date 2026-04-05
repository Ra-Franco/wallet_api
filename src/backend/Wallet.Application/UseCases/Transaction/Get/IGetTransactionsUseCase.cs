using Wallet.Communication.Requests.Transactions;
using Wallet.Communication.Responses.Transaction;
using Wallet.Domain.Utils.Page;

namespace Wallet.Application.UseCases.Transaction.Get
{
    public interface IGetTransactionsUseCase
    {
        public Task<PagedList<ResponseShortTransaction>> Execute(RequestTransactionsFilter request, PageParameters pageParameters);
    }
}
