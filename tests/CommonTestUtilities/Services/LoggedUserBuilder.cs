using Moq;
using Wallet.Domain.Entities;
using Wallet.Domain.Services.LoggedUser;

namespace CommonTestUtilities.Services
{
    public class LoggedUserBuilder
    {
        public static ILoggedUser Build(User user)
        {
            var mock = new Mock<ILoggedUser>();
            mock.Setup(x => x.User()).ReturnsAsync(user);

            return mock.Object;
        }
    }
}
