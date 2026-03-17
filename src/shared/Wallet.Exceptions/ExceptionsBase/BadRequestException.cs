using System.Net;
using Wallet.Exceptions.Wallet;

namespace Wallet.Exceptions.ExceptionsBase
{
    public class BadRequestException : WalletException
    {
        public BadRequestException(string message) : base(message)
        {
        }

        public override IList<string> GetErrorMessages() => [Message];

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
    }
}
