using Wallet.Domain.Repositories;

namespace Wallet.Infrasctructure.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WalletDbContext _dbContext;

        public UnitOfWork(WalletDbContext dbContext) => _dbContext = dbContext;

        public async Task Commit() => await _dbContext.SaveChangesAsync(); 
    }
}
