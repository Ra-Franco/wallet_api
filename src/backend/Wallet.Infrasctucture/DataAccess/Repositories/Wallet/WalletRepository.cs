using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Entities;
using Wallet.Domain.Repositories.Wallet;

namespace Wallet.Infrasctructure.DataAccess.Repositories.Wallet
{
    public class WalletRepository : IWalletWriteOnlyRepository, IWalletReadOnlyRepository
    {
        private readonly WalletDbContext _dbContext;

        public WalletRepository(WalletDbContext dbContext) => _dbContext = dbContext;

        public async Task CreateWallet(WalletEntity wallet)
        {
            await _dbContext.AddAsync(wallet);
        }

        public async Task<WalletEntity> FindWalletByUserId(long userId)
        {
            return await _dbContext
                .Wallet
                .FirstAsync(wallet => wallet.UserId == userId);
        }

        public async Task<WalletEntity?> GetWalletDashboard(long userId)
        {
            return await _dbContext
                .Wallet
                .FirstOrDefaultAsync(wallet => wallet.UserId == userId);
        }

        public async Task UpdateTransactionPassword(string transactionPassword, long walletId) => await _dbContext
                .Wallet
                .Where(w => w.Id == walletId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(w => w.TransactionPassword, transactionPassword));

        public async Task UpdateAmount(long walletId, decimal balance)
        {
            await _dbContext
                .Wallet
                .Where(w => w.Id == walletId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(w => w.Balance, balance));
        }

        public async Task<string?> GetTransactionalPasswordByUserId(long userId)
        {
            return await _dbContext
                .Wallet
                .Where(w => w.UserId.Equals(userId))
                .Select(w => w.TransactionPassword)
                .FirstOrDefaultAsync();
        }

        public async Task<WalletEntity?> FindWalletByCpf(string cpf)
        {
            return await _dbContext
                .Wallet
                .Join(_dbContext.Users,
                    w => w.UserId,
                    u => u.Id,
                    (w, u) => new { Wallet = w, User = u })
                .Where(x => x.User.CPF.Equals(cpf))
                .Select(x => x.Wallet)
                .FirstOrDefaultAsync();
        }

        public async Task<decimal> FindBalanceByUserId(long userId)
        {
            return await _dbContext
                .Wallet
                .Where(w => w.UserId == userId)
                .Select(w => w.Balance)
                .FirstAsync();
        }
    }
}
