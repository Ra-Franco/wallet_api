using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Entities;
using Wallet.Domain.Enum;
using Wallet.Domain.Repositories.User;
using Wallet.Infrasctucture.DataAccess;

namespace Wallet.Infrasctructure.DataAccess.Repositories.User
{
    public class UserRepository : IUserRepositoryReadOnly, IUserRepositoryWriteOnly
    {
        private readonly WalletDbContext _dbContext;

        public UserRepository(WalletDbContext dbContext) => _dbContext = dbContext;

        public async Task Add(Domain.Entities.User user) => await _dbContext.AddAsync(user);

        public async Task<bool> ExistUserWithCpf(string cpf)
        {
            return await _dbContext
                .Users
                .AsNoTracking()
                .AnyAsync(user => user.CPF.Equals(cpf));
        }

        public async Task<bool> ExistUserWithEmail(string email)
        {
            return await _dbContext
                .Users
                .AsNoTracking()
                .AnyAsync(user => user.Email.Equals(email));
        }

        public async Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier)
        {
            return await _dbContext
                .Users
                .AnyAsync(user => user.UserIdentifier.Equals(userIdentifier) && user.Status == UserStatus.Active);
        }

        public async Task<Domain.Entities.User?> ExistActiveUserWithCpfAndPassword(string cpf, string password) => await _dbContext
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.CPF == cpf && user.Password == password && user.Status == UserStatus.Active);

        public async Task Update(Domain.Entities.User user)
        {
            _dbContext
            .Users
            .Update(user);
        }
    }
}
