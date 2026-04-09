using CommonTestUtilities.Token;
using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Users.Registration
{
    public class GetUserRegistrationTest : WalletCustomClassFixture
    {

        private readonly string ROUTE = "user/registration";
        private readonly Guid _userIdentifier;
        public GetUserRegistrationTest(CustomWebApplicationFactory factory) : base(factory)
        {
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

            responseData.RootElement.GetProperty("email").GetString().Should().NotBeNullOrEmpty();
            responseData.RootElement.GetProperty("phonenumber").GetString().Should().NotBeNullOrEmpty();
            responseData.RootElement.GetProperty("income").GetString().Should().NotBeNullOrEmpty();
            responseData.RootElement.GetProperty("occupation").GetString().Should().NotBeNullOrEmpty();
        }
    }
}
