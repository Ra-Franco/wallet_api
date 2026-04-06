using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Wallet.Communication.Requests.Login;
using Wallet.Exceptions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Login.DoLogin
{
    public class DoLoginTest : WalletCustomClassFixture
    {
        private readonly string route = "auth/login";
        private readonly string _cpf;
        private readonly string _password;

        public DoLoginTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _cpf = factory.getCpf();
            _password = factory.getPassword();
        }

        [Fact]
        public async Task Success()
        {
            var request = new RequestLoginJson
            {
                Cpf = _cpf,
                Password = _password
            };
            var response = await DoPost(route, request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);
            responseData.RootElement.GetProperty("accessToken").GetString().Should().NotBeNullOrWhiteSpace();
            responseData.RootElement.GetProperty("refreshToken").GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Login_Invalid(string culture)
        {
            var request = RequestLoginJsonBuilder.Build();

            var response = await DoPost(route, request, culture);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);
            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessageException.ResourceManager.GetString("CPF_OR_PASSWORD_INCORRECT", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
