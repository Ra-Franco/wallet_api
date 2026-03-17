using System.Net;
using Wallet.Exceptions.Wallet;

namespace Wallet.Exceptions.ExceptionsBase
{
    public class UnathorizedException : WalletException
    {
        public UnathorizedException() : base(ResourceMessageException.UNAUTHORIZED_ERROR)
        {

        }

        public override IList<string> GetErrorMessages() => [Message];
        public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
    }
}
