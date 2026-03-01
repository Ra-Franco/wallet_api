namespace Wallet.Domain.Repositories.Wallet
{
    public interface IWalletWriteOnlyRepository
    {
        public Task CreateWallet(Entities.Wallet wallet);
    }
}
