namespace Wallet.Domain.Repositories
{
    public interface IUnitOfWork
    {
        public Task Commit();
    }
}
