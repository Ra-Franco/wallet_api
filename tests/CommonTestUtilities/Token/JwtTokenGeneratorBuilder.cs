using Wallet.Domain.Security.Tokens;
using Wallet.Infrasctructure.Security.Token.Access;

namespace CommonTestUtilities.Token
{
    public class JwtTokenGeneratorBuilder
    {
        public static IAccessTokenGenerator Build() => new JwtTokenGenerator(expirationTimeMinutes: 5, signingKey: "123123123123123123123123123123123123123123");
    }
}
