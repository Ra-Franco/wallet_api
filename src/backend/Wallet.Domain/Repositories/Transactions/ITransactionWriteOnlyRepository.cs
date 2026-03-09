using Wallet.Domain.Entities;

namespace Wallet.Domain.Repositories.Transactions
{
    public interface ITransactionWriteOnlyRepository
    {
        public Task Add(Transaction deposit);
    }
}
