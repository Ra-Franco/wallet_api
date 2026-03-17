using System.Net;

namespace Wallet.Exceptions.Wallet
{
    public abstract class WalletException : SystemException
    {
        public WalletException(string message) : base(message) { }

        public abstract IList<string> GetErrorMessages();
        public abstract HttpStatusCode GetStatusCode();
    }
}
