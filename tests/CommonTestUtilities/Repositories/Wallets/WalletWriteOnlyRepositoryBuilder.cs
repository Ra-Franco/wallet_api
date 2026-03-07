using Moq;
using Wallet.Domain.Repositories.User;
using Wallet.Domain.Repositories.Wallet;

namespace CommonTestUtilities.Repositories.Wallets
{
    public class WalletWriteOnlyRepositoryBuilder
    {
        public static IWalletWriteOnlyRepository Build()
        {
            var mock = new Mock<IWalletWriteOnlyRepository>();
            return mock.Object;
        }
    }
}
