using System.Net;

namespace Wallet.Exceptions.ExceptionsBase
{
    public class InvalidLoginException : WalletException
    {
        public InvalidLoginException() : base(ResourceMessageException.CPF_OR_PASSWORD_INCORRECT)
        {
        }

        public override IList<string> GetErrorMessages() => [Message];

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
    }
}
