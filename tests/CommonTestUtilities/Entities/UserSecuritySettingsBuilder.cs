using Bogus;
using Wallet.Domain.Entities;
using Wallet.Domain.Enum;

namespace CommonTestUtilities.Entities;

public class UserSecuritySettingsBuilder
{
    public static UserSecuritySettings Build(User user)
    {
        return new Faker<UserSecuritySettings>()
            .RuleFor(r => r.UserId, f => user.Id)
            .RuleFor(r => r.TransactionLimit, f => f.Random.Number(1, 100))
            .RuleFor(r => r.TransactionLimitPeriod, f => f.PickRandom<TransactionLimitPeriod>())
            .RuleFor(r => r.Id , f => user.Id);
    }
}