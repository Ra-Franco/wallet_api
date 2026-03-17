using System.Net;
using Wallet.Exceptions.Wallet;

namespace Wallet.Exceptions.TransactionalException
{
    public class AmountNotAvaliableExecption : WalletException
    {
        public AmountNotAvaliableExecption() : base(ResourceMessageException.AMOUNT_NOT_AVALIABLE)
        {
        }

        public override IList<string> GetErrorMessages() => [Message];

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.UnprocessableEntity;
    }
}
