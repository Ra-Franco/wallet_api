using Bogus;
using Wallet.Domain.Entities;
using Wallet.Domain.Enum;

namespace CommonTestUtilities.Entities
{
    public class WalletBuilder
    {
        public static Wallet.Domain.Entities.WalletEntity Build(User user, WalletStatus status = WalletStatus.Active)
        {
            return new Faker<Wallet.Domain.Entities.WalletEntity>()
                .RuleFor(w => w.UserId, _ => user.Id)
                .RuleFor(w => w.Balance, f => f.Finance.Amount(0, 5000))
                .RuleFor(w => w.PendingBalance, f => f.Finance.Amount(0, 1000))
                .RuleFor(w => w.TransactionPassword, f => f.Internet.Password(8))
                .RuleFor(w => w.Status, status)
                .RuleFor(w => w.Id, () => 1);
        }
    }
}
