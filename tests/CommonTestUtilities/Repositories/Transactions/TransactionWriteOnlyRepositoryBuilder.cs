using Moq;
using Wallet.Domain.Repositories.Transactions;

namespace CommonTestUtilities.Repositories.Transactions
{
    public class TransactionWriteOnlyRepositoryBuilder
    {
        public static ITransactionWriteOnlyRepository Build()
        {
           var mock = new Mock<ITransactionWriteOnlyRepository>();
            return mock.Object;
        }
    }
}
