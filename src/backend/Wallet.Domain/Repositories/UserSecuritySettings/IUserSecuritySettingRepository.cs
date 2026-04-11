namespace Wallet.Domain.Repositories.UserSecuritySettings
{
    public interface IUserSecuritySettingRepository
    {
        public Task<Entities.UserSecuritySettings?> GetSettingByUserId(long userId);
        public Task Add(Entities.UserSecuritySettings userSecuritySettings);
        public Task Update(Entities.UserSecuritySettings userSecuritySettings);
    }
}
