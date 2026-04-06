using Wallet.Domain.Security.Tokens;

namespace Wallet.Infrasctructure.Security.Token.Refresh
{
    public class RefreshTokenGenerator : IRefreshTokenGenerator
    {
        public string Generate() => Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }
}
