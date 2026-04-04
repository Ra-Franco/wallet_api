using CommonTestUtilities.Token;
using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Transactions.GetDeposits
{
    public class GetTransactionsTest : WalletCustomClassFixture
    {
        private readonly string ROUTE = "/transaction";
        private readonly Guid _userIdentifier;
        protected CustomWebApplicationFactory _factory { get; }
        public GetTransactionsTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _factory = factory;
            _userIdentifier = factory.getUserIdentifier();
        }

        [Fact]
        public async Task Success()
        {
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            var response = await DoGet(ROUTE, token, culture: "en");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);
            var items = responseData.RootElement.GetProperty("items");
            items.GetArrayLength().Should().BeGreaterThan(0);

            foreach (var item in items.EnumerateArray())
            {
                item.GetProperty("transactionNumber").GetString().Should().NotBeNull();
                item.GetProperty("transactionType").GetString().Should().NotBeNull();
                item.GetProperty("amount").GetDecimal().Should().BeGreaterThan(-1);
            }
        }
    }
}
