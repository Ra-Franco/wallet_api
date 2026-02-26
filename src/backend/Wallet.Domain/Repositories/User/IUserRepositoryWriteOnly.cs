namespace Wallet.Domain.Repositories.User
{
    public interface IUserRepositoryWriteOnly
    {
        public Task Add(Entities.User user);
    }
}
