using Wallet.Domain.Dtos;
using Wallet.Domain.Entities;
using Wallet.Domain.Utils.Page;

namespace Wallet.Domain.Repositories.Transactions
{
    public interface ITransactionReadOnlyRepository
    {
        public Task<PagedList<Transaction>> GetTransactionsByUserId(long userId, FilterTransactionsDto filters, PageParameters pageParameters);
    }
}
