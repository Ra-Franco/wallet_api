using System.Net;
using System.Text.Json;
using FluentAssertions;
using Wallet.Communication.Requests.Login.Token;

namespace WebApi.Test.Auth.RefreshToken
{
    public class RefreshTokenTest : WalletCustomClassFixture
    {
        private readonly string ROUTE = "auth/refresh-token";
        private readonly string _refreshToken = string.Empty;
        public RefreshTokenTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _refreshToken = factory.getRefreshTokenValue();
        }

        [Fact]
        public async Task Success()
        {
            var request = new RequestNewTokenJson{ RefreshToken = _refreshToken };

            var response = await DoPost(ROUTE, request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);
            responseData.RootElement.GetProperty("accessToken").GetString().Should().NotBeNullOrWhiteSpace();
            responseData.RootElement.GetProperty("refreshToken").GetString().Should().NotBeNullOrWhiteSpace();
        }
    }
}
