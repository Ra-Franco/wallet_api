using Moq;
using Wallet.Domain.Repositories.UserSecuritySettings;

namespace CommonTestUtilities.Repositories.UserSecuritySettings;

public class UserSecuritySettingsRepositoryBuilder
{
    private readonly Mock<IUserSecuritySettingRepository> _repository = new();

    public IUserSecuritySettingRepository Build() => _repository.Object;

    public void GetSettingByUserId(long userId, Wallet.Domain.Entities.UserSecuritySettings? userSecuritySettings)
    {
        _repository.Setup(repo => repo.GetSettingByUserId(userId)).ReturnsAsync(userSecuritySettings);
    }
}