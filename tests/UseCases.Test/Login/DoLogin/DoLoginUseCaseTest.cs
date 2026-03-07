using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.User;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Token;
using FluentAssertions;
using Wallet.Application.UseCases.Auth.Login;
using Wallet.Communication.Requests.Login;
using Wallet.Exceptions;
using Wallet.Exceptions.ExceptionsBase;

namespace UseCases.Test.Login.DoLogin
{
    public class DoLoginUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, var password) = UserBuilder.Build();
            var useCase = CreateUseCase(user);

            var result = await useCase.Execute(new RequestLoginJson
            {
                Cpf = user.CPF,
                Password = password
            });

            result.Should().NotBeNull();
            result.AccessToken.Should().NotBeNull();
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

        private DoLoginUseCase CreateUseCase(Wallet.Domain.Entities.User? user = null)
        {
            var repository = new UserReadOnlyRepositoryBuilder();
            var passwordEncrypter = PasswordEncrypterBuilder.Build();
            var accessToken = JwtTokenGeneratorBuilder.Build();

            if (user != null)
                repository.ExistActiveUserWithCpfAndPassword(user);

            return new DoLoginUseCase(repository.Build(), passwordEncrypter, accessToken);
        }
    }
}
