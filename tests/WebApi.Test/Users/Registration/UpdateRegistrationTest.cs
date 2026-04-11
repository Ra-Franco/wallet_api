using CommonTestUtilities.Requests;
using CommonTestUtilities.Token;
using FluentAssertions;
using System.Net;

namespace WebApi.Test.Users.Registration
{
    public class UpdateRegistrationTest : WalletCustomClassFixture
    {
        private readonly string ROUTE = "user/registration";
        private readonly Guid _userIdentifier;
        protected CustomWebApplicationFactory _factory { get; }
        public UpdateRegistrationTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _factory = factory;
            _userIdentifier = factory.getUserIdentifier();
        }

        [Fact]
        public async Task Success()
        {
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);
            var request = RequestUpdateRegistrationUserBuilder.JsonPatchBuild();
            var response = await DoPatch(route: ROUTE, request: request, token: token);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
