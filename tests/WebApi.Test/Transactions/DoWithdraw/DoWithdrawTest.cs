using Bogus;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Token;
using FluentAssertions;
using Microsoft.AspNetCore.Routing;
using System.Globalization;
using System.Net;
using System.Text.Json;
using Wallet.Domain.Enum;
using Wallet.Exceptions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Transactions.DoWithdraw
{
    public class DoWithdrawTest : WalletCustomClassFixture
    {
        private readonly string ROUTE = "transaction/withdraw";
        private readonly Guid _userIdentifier;
        private readonly string _transactionalPassword;
        private readonly string _cpfReceiver;
        private readonly Faker _faker;
        protected CustomWebApplicationFactory _factory { get; }
        public DoWithdrawTest(CustomWebApplicationFactory factory): base(factory)
        {
            _factory = factory;
            _userIdentifier = factory.getUserIdentifier();
            _transactionalPassword = factory.getTransactionalPassword();
            _cpfReceiver = factory.getCpfReceiver();
            _faker = new Faker();
        }

        [Fact]
        public async Task Success()
        {
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);
            var request = RequestCreateWithdrawBuilder.Build();

            request.TransactionPassword = _transactionalPassword;

            var response = await DoPost(ROUTE, request, token: token);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);
            responseData.RootElement.GetProperty("amount").GetDecimal().Should().BeGreaterThan(-1);
            responseData.RootElement.GetProperty("status").GetString().Should().Be(TransactionStatus.Completed.ToString());
            responseData.RootElement.GetProperty("transactionNumber").GetString().Should().NotBeNullOrEmpty();
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Amount_Invalid(string culture)
        {
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);
            var request = RequestCreateWithdrawBuilder.Build();

            request.TransactionPassword = _transactionalPassword;
            request.Amount = decimal.MaxValue.ToString(CultureInfo.InvariantCulture);

            var response = await DoPost(ROUTE, request, token: token, culture: culture);

            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableContent);
            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);
            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessageException.ResourceManager.GetString("AMOUNT_NOT_AVALIABLE", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Transactional_Password(string culture)
        {
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);
            var request = RequestCreateWithdrawBuilder.Build();

            request.TransactionPassword = _faker.Internet.Password(6, regexPattern: @"^\d+$");

            var response = await DoPost(ROUTE, request, token: token, culture: culture);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);
            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessageException.ResourceManager.GetString("TRANSACTIONAL_PASSWORD_INVALID", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
