using CommonTestUtilities.Token;
using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Wallets.GetBalance
{
    public class GetBalanceTest : WalletCustomClassFixture
    {
        private readonly string ROUTE = "wallet/balance";
        private readonly Guid _userIdentifier;
        public GetBalanceTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIdentifier = factory.getUserIdentifier();
        }

        [Fact]
        public async Task Success()
        {
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            var response = await DoGet(ROUTE, token: token);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);
            responseData.RootElement.GetProperty("balance").GetDecimal().Should().BeGreaterThan(-1);
        }
    }
}
