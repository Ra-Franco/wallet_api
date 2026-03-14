using Bogus;
using Wallet.Domain.Entities;
using Wallet.Domain.Enum;

namespace CommonTestUtilities.Entities
{
    public class TransactionBuilder
    {
        public static Transaction Build(long walletId)
        {
            return new Faker<Transaction>()
                .RuleFor(t => t.Id, f => f.Random.Long(1))
                .RuleFor(t => t.Amount, f => f.Finance.Amount())
                .RuleFor(t => t.TransactionDate, f => f.Date.Recent(15))
                .RuleFor(t => t.Type, f => f.PickRandom<TransactionType>())
                .RuleFor(t => t.Status, f => f.PickRandom<TransactionStatus>())
                .RuleFor(t => t.WalletId, walletId)
                .Generate();
        }

        public static List<Transaction> BuildList(long walletId, int count = 10) =>
            Enumerable.Range(0, count).Select(_ => Build(walletId)).ToList();
    }
}
