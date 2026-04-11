using Bogus;
using Bogus.Extensions.Brazil;
using Wallet.Communication.Requests.Login;

namespace CommonTestUtilities.Requests
{
    public class RequestLoginJsonBuilder
    {
        public static RequestLoginJson Build()
        {
            return new Faker<RequestLoginJson>()
                .RuleFor(user => user.Cpf, (f) => f.Person.Cpf())
                .RuleFor(user => user.Password, (f) => f.Internet.Password());
        }
    }
}
