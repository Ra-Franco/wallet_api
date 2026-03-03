using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Repositories.Wallet;
using Wallet.Infrasctucture.DataAccess;

namespace Wallet.Infrasctructure.DataAccess.Repositories.Wallet
{
    public class WalletRepository : IWalletWriteOnlyRepository, IWalletReadOnlyRepository
    {
        private readonly WalletDbContext _dbContext;

        public WalletRepository(WalletDbContext dbContext) => _dbContext = dbContext;

        public async Task CreateWallet(Domain.Entities.Wallet wallet)
        {
            await _dbContext.AddAsync(wallet);
        }

        public async Task<Domain.Entities.Wallet> FindWalletByUserId(long userId)
        {
            return await _dbContext
                .Wallet
                .FirstAsync(wallet => wallet.UserId == userId);
        }

        public async Task<Domain.Entities.Wallet?> GetWalletDashboard(long userId)
        {
            return await _dbContext
                .Wallet
                .FirstOrDefaultAsync(wallet => wallet.UserId == userId);
        }

        public async Task UpdateTransactionPassword(string transactionPassword, long walletId)
        {
            await _dbContext
                .Wallet
                .Where(wallet => wallet.Id == walletId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(wallet => wallet.TransactionPassword, transactionPassword)
                    .SetProperty(wallet => wallet.Last_Updated, DateTime.UtcNow)
                );
        }
    }
}
