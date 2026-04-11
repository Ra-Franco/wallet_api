using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Repositories.UserSecuritySettings;

namespace Wallet.Infrasctructure.DataAccess.Repositories.UserSecuritySettings
{
    internal class UserSecuritySettingRepository : IUserSecuritySettingRepository
    {
        private readonly WalletDbContext _dbContext;

        public UserSecuritySettingRepository(WalletDbContext dbContext) => _dbContext = dbContext;

        public async Task Add(Domain.Entities.UserSecuritySettings userSecuritySettings)
        {
            await _dbContext.
                UserSecuritySettings
                .AddAsync(userSecuritySettings);
        }

        public async Task<Domain.Entities.UserSecuritySettings?> GetSettingByUserId(long userId)
        {
            return await _dbContext.UserSecuritySettings
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task Update(Domain.Entities.UserSecuritySettings userSecuritySettings)
        {
            _dbContext
                .UserSecuritySettings
                .Update(userSecuritySettings);
        }
    }
}
