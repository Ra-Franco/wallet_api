using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Repositories.User;
using Wallet.Infrasctucture.DataAccess;

namespace Wallet.Infrasctructure.DataAccess.Repositories.User
{
    public class UserRepository : IUserRepositoryReadOnly, IUserRepositoryWriteOnly
    {
        private readonly WalletDbContext _dbContext;

        public UserRepository(WalletDbContext dbContext) => _dbContext = dbContext;

        public async Task Add(Domain.Entities.User user) => await _dbContext.AddAsync(user);

        public async Task<bool> ExistActiveUserWithCpf(string cpf)
        {
            return await _dbContext.Users.AnyAsync(user => user.CPF.Equals(cpf));
        }

        public async Task<bool> ExistActiveUserWithEmail(string email)
        {
            return await _dbContext.Users.AnyAsync(user => user.Email.Equals(email));
        }

        public async Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier)
        {
            return await _dbContext
                .Users
                .AnyAsync(user => user.UserIdentifier.Equals(userIdentifier) && user.Status.Equals("Active"));
        }
    }
}
