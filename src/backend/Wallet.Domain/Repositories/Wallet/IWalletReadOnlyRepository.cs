namespace Wallet.Domain.Repositories.Wallet
{
    public interface IWalletReadOnlyRepository
    {
        public Task<Entities.WalletEntity> FindWalletByUserId(long userId);

        public Task<Entities.WalletEntity> GetWalletDashboard(long userId);
    }
}
