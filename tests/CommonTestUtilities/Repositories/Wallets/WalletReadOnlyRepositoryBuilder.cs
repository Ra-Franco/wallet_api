using Moq;
using Wallet.Domain.Repositories.Wallet;

namespace CommonTestUtilities.Repositories.Wallets
{
    public class WalletReadOnlyRepositoryBuilder
    {
        private readonly Mock<IWalletReadOnlyRepository> _repository;

        public WalletReadOnlyRepositoryBuilder() => _repository = new Mock<IWalletReadOnlyRepository>();

        public IWalletReadOnlyRepository Build() => _repository.Object;

        public void FindWalletByUserId(Wallet.Domain.Entities.WalletEntity wallet)
        {
            _repository.Setup(repo => repo.FindWalletByUserId(wallet.UserId)).ReturnsAsync(wallet);
        }

        public void GetWalletDashboard(Wallet.Domain.Entities.WalletEntity wallet)
        {
            _repository.Setup(repo => repo.GetWalletDashboard(wallet.Id)).ReturnsAsync(wallet);
        }
    }
}
