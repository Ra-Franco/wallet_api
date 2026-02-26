using Wallet.Domain.Entities;

namespace Wallet.Application.Services.LoggedUser
{
    public interface ILoggedUser
    {
        public Task<User> User();
    }
}
