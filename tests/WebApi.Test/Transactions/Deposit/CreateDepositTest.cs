using CommonTestUtilities.Requests;
using CommonTestUtilities.Token;
using FluentAssertions;
using System.Net;
using System.Text.Json;
using Wallet.Communication.Requests.Deposit;
using Wallet.Domain.Enum;
using Wallet.Exceptions;
using Wallet.Infrasctucture.DataAccess;

namespace WebApi.Test.Transactions.Deposit
{
    public class CreateDepositTest : WalletCustomClassFixture
    {
        private readonly string ROUTE = "transaction/deposit";
        private readonly Guid _userIdentifier;
        protected CustomWebApplicationFactory _factory { get; }
        public CreateDepositTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _factory = factory;
            _userIdentifier = factory.getUserIdentifier();
        }

        [Fact]
        public async Task Success()
        {
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            var request = RequestCreateDepositBuilder.Build();

            var response = await DoPost(ROUTE, request, token: token);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);
            responseData.RootElement.GetProperty("amount").GetDecimal().Should().BeGreaterThan(-1);
            responseData.RootElement.GetProperty("status").GetString().Should().Be(TransactionStatus.Completed.ToString());
            responseData.RootElement.GetProperty("transactionNumber").GetString().Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Error_Validator_ReturnsBadRequest_WhenDescriptionTooLong()
        {
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);
            var request = RequestCreateDepositBuilder.Build(500);

            var response = await DoPost(ROUTE, request, token: token, culture: "pt-BR");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var body = await response.Content.ReadAsStringAsync();
            body.Should().Contain(ResourceMessageException.TRANSACTIONAL_DESCRIPTION_MAX_LENGTH);
        }

        [Fact]
        public async Task RequireActiveWallet_ReturnsForbidden_WhenWalletIsNotActive()
        {

            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);
            var request = RequestCreateDepositBuilder.Build();
            _factory.SetWalletStatus(_userIdentifier, WalletStatus.Suspended);


            var response = await DoPost(ROUTE, request, token: token, culture: "pt-BR");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            var body = await response.Content.ReadAsStringAsync();
            body.Should().Contain(ResourceMessageException.WALLET_NOT_ACTIVE);
        }
    }
}
