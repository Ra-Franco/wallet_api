using Wallet.Domain.Entities;

namespace Wallet.Domain.Services.LoggedUser
{
    public interface ILoggedUser
    {
        public Task<User> User();
    }
}
