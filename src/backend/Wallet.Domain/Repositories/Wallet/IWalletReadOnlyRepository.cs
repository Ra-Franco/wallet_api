using Wallet.Domain.Entities;

namespace Wallet.Domain.Repositories.Wallet
{
    public interface IWalletReadOnlyRepository
    {
        public Task<WalletEntity> FindWalletByUserId(long userId);

        public Task<WalletEntity?> GetWalletDashboard(long userId);

        public Task<string?> GetTransactionalPasswordByUserId(long userId);

        public Task<WalletEntity?> FindWalletByCpf(string cpf);

        public Task<decimal> FindBalanceByUserId(long userId);
    }
}
