using System.Net;
using Wallet.Exceptions.Wallet;

namespace Wallet.Exceptions.ExceptionsBase
{
    public class ErrorOnValidationException : WalletException
    {
        private readonly IList<string> _errorMessages;

        public ErrorOnValidationException(IList<string> errors) : base(string.Empty)
        {
            _errorMessages = errors;
        }

        public override IList<string> GetErrorMessages() => _errorMessages;

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest; 
    }
}
