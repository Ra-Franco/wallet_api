using CommonTestUtilities.Requests;
using CommonTestUtilities.Token;
using FluentAssertions;
using System.Net;

namespace WebApi.Test.Wallets.SetTransactionalPassword
{
    public class SetTransactionPasswordTest : WalletCustomClassFixture
    {
        private readonly string ROUTE = "wallet/set-transactional-password";
        private readonly Guid _userIdentifier;
        public SetTransactionPasswordTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIdentifier = factory.getUserIdentifier();
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestSetTransactionPasswordJsonBuilder.Build();
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            var response = await DoPut(ROUTE, request, token: token);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Error_Unathorized()
        {
            var request = RequestSetTransactionPasswordJsonBuilder.Build();
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);
            token = string.Empty;
            var response = await DoPut(ROUTE, request, token: token);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
