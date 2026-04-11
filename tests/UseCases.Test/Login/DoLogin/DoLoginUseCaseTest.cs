using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Token;
using CommonTestUtilities.Repositories.Users;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services;
using CommonTestUtilities.Token;
using FluentAssertions;
using Wallet.Application.UseCases.Auth.Login;
using Wallet.Communication.Requests.Login;
using Wallet.Domain.Entities;
using Wallet.Exceptions;
using Wallet.Exceptions.Login;

namespace UseCases.Test.Login.DoLogin
{
    public class DoLoginUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, var password) = UserBuilder.Build();
            var refreshToken = RefreshTokenBuilder.Build(user);
            var useCase = CreateUseCase(refreshToken, user);

            var result = await useCase.Execute(new RequestLoginJson
            {
                Cpf = user.CPF,
                Password = password
            });

            result.Should().NotBeNull();
            result.AccessToken.Should().NotBeNull();
            result.RefreshToken.Should().NotBeNull();
        }

        [Fact]
        public async Task Error_Invalid_User()
        {
            var request = RequestLoginJsonBuilder.Build();

            var useCase = CreateUseCase();

            Func<Task> act = async () => { await useCase.Execute(request); };

            await act.Should().ThrowAsync<InvalidLoginException>()
                .Where(e => e.Message.Equals(ResourceMessageException.CPF_OR_PASSWORD_INCORRECT));
        }

        private DoLoginUseCase CreateUseCase(RefreshToken? refreshToken = null, User? user = null)
        {
            var repository = new UserReadOnlyRepositoryBuilder();
            var passwordEncrypter = PasswordEncrypterBuilder.Build();
            var accessToken = JwtTokenGeneratorBuilder.Build();

            if (user != null)
                repository.ExistActiveUserWithCpfAndPassword(user);

            var unitOfWork = UnitOfWorkBuilder.Build();
            var refreshTokenGenerator = RefreshTokenGeneratorBuilder.Build();
            var tokenRepo = new TokenRepositoryBuilder();

            if (refreshToken != null)
                tokenRepo.Get(refreshToken);

            return new DoLoginUseCase(repository.Build(), passwordEncrypter, accessToken,refreshTokenGenerator,tokenRepo.Build(),unitOfWork);
        }
    }
}
