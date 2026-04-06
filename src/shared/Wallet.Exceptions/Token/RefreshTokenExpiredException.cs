using System.Net;
using Wallet.Exceptions.Wallet;

namespace Wallet.Exceptions.Token
{
    public class RefreshTokenExpiredException : WalletException
    {
        public RefreshTokenExpiredException() : base(ResourceMessageException.REFRESH_TOKEN_EXPIRED)
        {
        }

        public override IList<string> GetErrorMessages() => [Message];

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
    }
}
