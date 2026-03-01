using Wallet.Domain.Repositories.Wallet;
using Wallet.Infrasctucture.DataAccess;

namespace Wallet.Infrasctructure.DataAccess.Repositories.Wallet
{
    public class WalletRepository : IWalletWriteOnlyRepository
    {
        private readonly WalletDbContext _dbContext;

        public WalletRepository(WalletDbContext dbContext) => _dbContext = dbContext;

        public async Task CreateWallet(Domain.Entities.Wallet wallet)
        {
            await _dbContext.AddAsync(wallet);
        }
    }
}
