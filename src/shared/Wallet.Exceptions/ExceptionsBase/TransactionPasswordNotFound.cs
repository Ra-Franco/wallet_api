using System.Net;

namespace Wallet.Exceptions.ExceptionsBase
{
    public class TransactionPasswordNotFound : NotFoundException
    {
        public TransactionPasswordNotFound() : base(ResourceMessageException.TRANSACTIONAL_PASSWORD_IS_NULL)
        {
        }
    }
}
