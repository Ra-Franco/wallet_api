using Moq;
using Wallet.Domain.Security.Tokens;

namespace CommonTestUtilities.Services
{
    public class RefreshTokenGeneratorBuilder
    {
        public static IRefreshTokenGenerator Build()
        {
            var mock = new Mock<IRefreshTokenGenerator>();
            mock.Setup(m => m.Generate()).Returns(Convert.ToBase64String(Guid.NewGuid().ToByteArray()));
            return mock.Object;
        }
    }
}
