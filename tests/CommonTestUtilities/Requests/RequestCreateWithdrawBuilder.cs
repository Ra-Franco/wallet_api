using Bogus;
using Wallet.Communication.Requests.Transactions.Withdraw;
using Wallet.Communication.Utils;

namespace CommonTestUtilities.Requests
{
    public class RequestCreateWithdrawBuilder
    {
        public static RequestCreateWithdraw Build(int length = 6, int characteres = 3)
        {
            return new Faker<RequestCreateWithdraw>()
                .RuleFor(req => req.TransactionPassword, (f) => f.Internet.Password(length, regexPattern: @"^\d+$"))
                .RuleFor(req => req.Amount, (f) => f.Finance.Amount(0, 100).DecimalToStringCurrency())
                .RuleFor(req => req.Description, f => f.Lorem.Paragraph(characteres));
        }
    }
}
