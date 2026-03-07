using Bogus;
using Wallet.Communication.Requests.Wallet;

namespace CommonTestUtilities.Requests
{
    public class RequestSetTransactionPasswordJsonBuilder
    {
        public static RequestSetTransactionPasswordJson Build(int length = 6)
        {
            return new Faker<RequestSetTransactionPasswordJson>()
                .RuleFor(req => req.TransactionPassword, (f) => f.Internet.Password(length, regexPattern: @"^\d+$"));
        }
    }
}
