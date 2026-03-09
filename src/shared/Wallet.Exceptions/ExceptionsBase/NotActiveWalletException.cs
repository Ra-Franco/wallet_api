using System.Net;

namespace Wallet.Exceptions.ExceptionsBase
{
    public class NotActiveWalletException : WalletException
    {
        public NotActiveWalletException() : base(ResourceMessageException.WALLET_NOT_ACTIVE)
        {
        }

        public override IList<string> GetErrorMessages() => [Message];

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.Forbidden;
    }
}
