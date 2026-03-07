using CommonTestUtilities.Requests;
using CommonTestUtilities.Token;
using FluentAssertions;
using System.Net;

namespace WebApi.Test.Wallets.CreateTransactionalPassword
{
    public class CreateTransactionPasswordTest : WalletCustomClassFixture
    {
        private readonly string ROUTE = "create-transactional-password";
        private readonly Guid _userIdentifier;
        public CreateTransactionPasswordTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIdentifier = factory.getUserIdentifier();
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestSetTransactionPasswordJsonBuilder.Build();
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            var response = await DoPost(ROUTE, request, token: token);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Error_Unathorized()
        {
            var request = RequestSetTransactionPasswordJsonBuilder.Build();
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);
            token = string.Empty;
            var response = await DoPost(ROUTE, request, token: token);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
