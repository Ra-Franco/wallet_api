namespace Wallet.Domain.Security.TransferPassword
{
    public interface ITransferPasswordValidator
    {
        public Task Validate(long userId, string transactionalPassword);
    }
}
