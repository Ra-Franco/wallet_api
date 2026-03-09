using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Entities;
using Wallet.Domain.Repositories.Transactions;
using Wallet.Infrasctucture.DataAccess;

namespace Wallet.Infrasctructure.DataAccess.Repositories.Transactions
{
    public class TransactionRepository : ITransactionReadOnlyRepository, ITransactionWriteOnlyRepository
    {
        private readonly WalletDbContext _dbContext;

        public TransactionRepository(WalletDbContext dbContext) => _dbContext = dbContext;

        public async Task Add(Transaction deposit) => await _dbContext.AddAsync(deposit);

        public Task<bool> ExistsTransactionNumber(string transactionNumber)
        {
            return _dbContext
                .Transactions
                .AnyAsync(tr => tr.TransactionNumber == transactionNumber);

        }
    }
}
