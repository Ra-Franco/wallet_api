using System.Net;

namespace Wallet.Exceptions.ExceptionsBase
{
    public class NotFoundException : WalletException
    {
        public NotFoundException(string message) : base(message)
        {
        }

        public override IList<string> GetErrorMessages() => [Message];

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;
    }
}
