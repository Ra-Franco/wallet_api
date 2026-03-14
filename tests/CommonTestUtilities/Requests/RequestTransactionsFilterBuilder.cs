using Bogus;
using Wallet.Communication.Requests.Transactions;
using Wallet.Domain.Enum;

namespace CommonTestUtilities.Requests
{
    public class RequestTransactionsFilterBuilder
    {
        public static RequestTransactionsFilter Build()
        {

            return new Faker<RequestTransactionsFilter>()
                .RuleFor(r => r.Status, f => new List<TransactionStatus> { f.PickRandom<TransactionStatus>() })
                .RuleFor(r => r.Type, f => new List<TransactionType> { f.PickRandom<TransactionType>() });
        } 
    }
}
