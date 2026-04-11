using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Dtos;
using Wallet.Domain.Entities;
using Wallet.Domain.Repositories.Transactions;
using Wallet.Domain.Utils.Page;
using Wallet.Infrasctructure.Utils.Page;

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

        public async Task<PagedList<Transaction>> GetTransactionsByUserId(long userId,FilterTransactionsDto filter, PageParameters pageParameters)
        {
            var query = _dbContext
                .Transactions
                .Where(t => t.Wallet!.UserId == userId)
                .AsQueryable();

            if (filter.StartDate.HasValue)
                query = query.Where(t => t.TransactionDate >= filter.StartDate.Value.Date);

            if (filter.EndDate.HasValue)
                query = query.Where(t => t.TransactionDate <= filter.EndDate.Value.Date.AddDays(1).AddTicks(-1));

            if (filter.Type != null && filter.Type.Any())
                query = query.Where(t => filter.Type.Contains(t.Type));

            if (filter.Status != null && filter.Status.Any())
            {
                query = query.Where(t => filter.Status.Contains(t.Status));
            }

            var items = query
                .OrderByDescending(t => t.TransactionDate);

            return await PagedListExtensions.ToPagedListAsync(items, pageParameters.PageNumber, pageParameters.PageSize);
        }

        public async Task<Transaction?> GetByTransactionNumber(string transactionNumber, long walletId)
        {
            return await _dbContext.Transactions
                .Where(t => t.TransactionNumber == transactionNumber && (t.WalletId == walletId || t.RelatedWalletId == walletId))
                .FirstOrDefaultAsync();
        }
    }
}
