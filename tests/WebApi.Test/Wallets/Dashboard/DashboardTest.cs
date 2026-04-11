using CommonTestUtilities.Token;
using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Wallets.Dashboard
{
    public class DashboardTest : WalletCustomClassFixture
    {
        private readonly string ROUTE = "wallet";
        private readonly Guid _userIdentifier;
        public DashboardTest(CustomWebApplicationFactory factory) : base(factory)
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
            responseData.RootElement.GetProperty("pendingBalance").GetDecimal().Should().BeGreaterThan(-1);
            responseData.RootElement.GetProperty("status").GetString().Should().NotBeNullOrWhiteSpace();
            responseData.RootElement.GetProperty("id").GetDecimal().Should().BeGreaterThan(-1);
        }
        
        [Fact]
        public async Task Error_Unathorized()
        {
            string token = token = string.Empty;
            var response = await DoGet(ROUTE, token: token);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
