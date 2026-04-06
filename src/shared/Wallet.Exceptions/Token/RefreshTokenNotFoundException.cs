using System.Net;
using Wallet.Exceptions.Wallet;

namespace Wallet.Exceptions.Token
{
    public class RefreshTokenNotFoundException : WalletException
    {
        public RefreshTokenNotFoundException() : base(ResourceMessageException.REFRESH_TOKEN_NOT_FOUND)
        {
        }

        public override IList<string> GetErrorMessages() => [Message];

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
    }
}
