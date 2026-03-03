namespace Wallet.Domain.Repositories.Wallet
{
    public interface IWalletReadOnlyRepository
    {
        public Task<Entities.Wallet> FindWalletByUserId(long userId);

        public Task<Entities.Wallet> GetWalletDashboard(long userId);
    }
}
