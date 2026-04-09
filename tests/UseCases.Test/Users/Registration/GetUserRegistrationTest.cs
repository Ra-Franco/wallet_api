using CommonTestUtilities.Entities;
using CommonTestUtilities.Services;
using FluentAssertions;
using Wallet.Application.UseCases.User.Registration.Get;
using Wallet.Communication.Utils;
using Wallet.Domain.Entities;

namespace UseCases.Test.Users.Registration
{
    public class GetUserRegistrationTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, var _) = UserBuilder.Build();
            var useCase = CreateUseCase(user);

            var response = await useCase.Execute();

            response.Email.Should().Be(user.Email);
            response.Occupation.Should().Be(user.Occupation);
            response.Phonenumber.Should().Be(user.Phonenumber);
            response.Income.Should().Be(user.Income.DecimalToStringCurrency());
        }

        private static GetUserRegistration CreateUseCase(User user)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var mapper = MapperBuilder.Build();

            return new GetUserRegistration(loggedUser, mapper);
        }
    }
}
