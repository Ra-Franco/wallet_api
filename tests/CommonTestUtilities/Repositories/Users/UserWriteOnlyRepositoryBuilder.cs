using Moq;
using Wallet.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories.Users
{
    public class UserWriteOnlyRepositoryBuilder
    {
        public static IUserRepositoryWriteOnly Build()
        {
            var mock = new Mock<IUserRepositoryWriteOnly>();
            return mock.Object;
        }
    }
}
