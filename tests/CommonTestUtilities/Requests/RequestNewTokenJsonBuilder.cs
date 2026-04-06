using Bogus;
using Wallet.Communication.Requests.Login.Token;

namespace CommonTestUtilities.Requests
{
    public class RequestNewTokenJsonBuilder
    {
        public static RequestNewTokenJson Build()
        {
            return new Faker<RequestNewTokenJson>()
                    .RuleFor(r => r.RefreshToken, Guid.NewGuid().ToString());
        }
    }
}
