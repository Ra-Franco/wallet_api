using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Entities;
using Wallet.Domain.Repositories.Wallet;
using Wallet.Infrasctucture.DataAccess;

namespace Wallet.Infrasctructure.DataAccess.Repositories.Wallet
{
    public class WalletRepository : IWalletWriteOnlyRepository, IWalletReadOnlyRepository
    {
        private readonly WalletDbContext _dbContext;

        public WalletRepository(WalletDbContext dbContext) => _dbContext = dbContext;

        public async Task CreateWallet(Domain.Entities.WalletEntity wallet)
        {
            await _dbContext.AddAsync(wallet);
        }

        public async Task<Domain.Entities.WalletEntity> FindWalletByUserId(long userId)
        {
            return await _dbContext
                .Wallet
                .FirstAsync(wallet => wallet.UserId == userId);
        }

        public async Task<Domain.Entities.WalletEntity?> GetWalletDashboard(long userId)
        {
            return await _dbContext
                .Wallet
                .FirstOrDefaultAsync(wallet => wallet.UserId == userId);
        }

        public async Task UpdateTransactionPassword(string transactionPassword, WalletEntity wallet) => _dbContext
                .Wallet
                .Update(wallet);

        public async Task Update(WalletEntity wallet)
        {
            _dbContext
                .Wallet
                .Update(wallet);
        }
    }
}
