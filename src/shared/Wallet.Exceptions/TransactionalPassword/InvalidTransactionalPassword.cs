using Wallet.Exceptions.ExceptionsBase;

namespace Wallet.Exceptions.TransactionalPassword
{
    public class InvalidTransactionalPassword : BadRequestException
    {
        public InvalidTransactionalPassword() : base(ResourceMessageException.TRANSACTIONAL_PASSWORD_INVALID)
        {
        }
    }
}
