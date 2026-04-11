using CommonTestUtilities.Token;
using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Transactions.GetTransactionByTransactionNumber
{
    public class GetTransactionByTransactionNumber : WalletCustomClassFixture
    {
        private readonly string ROUTE = "/transaction/{transactionNumber}";
        private readonly Guid _userIdentifier;
        private readonly string _transactionNumber;
        protected CustomWebApplicationFactory _factory;
        public GetTransactionByTransactionNumber(CustomWebApplicationFactory factory) : base(factory)
        {
            _factory = factory;
            _userIdentifier = factory.getUserIdentifier();
            _transactionNumber = factory.getTransactionNumber();
        }

        [Fact]
        public async Task Success()
        {
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);
            var replacedRoute = ROUTE.Replace("{transactionNumber}", _transactionNumber);

            var response = await DoGet(replacedRoute, token, culture: "en");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            await using var responseBody = await response.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("transactionNumber").GetString().Should().Be(_transactionNumber);
            responseData.RootElement.GetProperty("amount").GetDecimal().Should().BeGreaterThan(-1);
            responseData.RootElement.GetProperty("status").GetString().Should().NotBeNullOrEmpty();

        }
    }
}
