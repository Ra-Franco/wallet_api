using Bogus;
using Bogus.Extensions.Brazil;
using Wallet.Communication.Requests.Transactions.Transfer;

namespace CommonTestUtilities.Requests
{
    public class RequestTransferBuilder
    {
        public static RequestTransfer Build(int length = 6, int characteres = 3)
        {
            return new Faker<RequestTransfer>()
                .RuleFor(req => req.TransactionPassword, (f) => f.Internet.Password(length, regexPattern: @"^\d+$"))
                .RuleFor(req => req.ReceiverCpf, (f) => f.Person.Cpf())
                .RuleFor(req => req.Amount, (f) => f.Finance.Amount(0, 100))
                .RuleFor(req => req.Description, f => f.Lorem.Paragraph(characteres));
        }
    }
}
