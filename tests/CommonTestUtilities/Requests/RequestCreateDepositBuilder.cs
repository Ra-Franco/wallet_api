using Bogus;
using Wallet.Communication.Requests.Transactions.Deposit;
using Wallet.Communication.Utils;

namespace CommonTestUtilities.Requests
{
    public class RequestCreateDepositBuilder
    {
        public static RequestCreateDeposit Build(int characteres = 3)
        {
            return new Faker<RequestCreateDeposit>()
                .RuleFor(d => d.Amount, (f) => f.Finance.Amount(0, 1000).DecimalToStringCurrency())
                .RuleFor(d => d.Description, f => f.Lorem.Paragraph(characteres));                
        }
    }
}
