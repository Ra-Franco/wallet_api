using Wallet.Exceptions.ExceptionsBase;

namespace Wallet.Exceptions.TransactionalPassword
{
    public class TransactionPasswordNotFound : NotFoundException
    {
        public TransactionPasswordNotFound() : base(ResourceMessageException.TRANSACTIONAL_PASSWORD_IS_NULL)
        {
        }
    }
}
