using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Token;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services;
using CommonTestUtilities.Token;
using CommonTestUtilities.UseCases;
using FluentAssertions;
using CommonTestUtilities.Repositories.Users;
using Wallet.Application.UseCases.User.Register;
using Wallet.Domain.Entities;
using Wallet.Exceptions;
using Wallet.Exceptions.ExceptionsBase;

namespace UseCases.Test.Users.Register
{
    public class RegisterUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            (var user, var _) = UserBuilder.Build();

            var refreshToken = RefreshTokenBuilder.Build(user);

            var useCase = CreateUseCase(tokenRefresh: refreshToken);

            
            var result = await useCase.Execute(request);

            result.Should().NotBeNull();
            result.Name.Should().Be(request.Name);
            result.Tokens.AccessToken.Should().NotBeNull();
        }

        [Fact]
        public async Task Error_Email_Already_Registered()
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            var useCase = CreateUseCase(request.Email);

            Func<Task> act = async () => await useCase.Execute(request);
            (await act.Should().ThrowAsync<ErrorOnValidationException>())
                .Where(e => e.GetErrorMessages().Count == 1 &&
                        e.GetErrorMessages().Contains(ResourceMessageException.EMAIL_ALREADY_REGISTERED));
        }
        [Fact]
        public async Task Cpf_Email_Already_Registered()
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            var useCase = CreateUseCase(string.Empty, request.CPF);

            Func<Task> act = async () => await useCase.Execute(request);
            (await act.Should().ThrowAsync<ErrorOnValidationException>())
                .Where(e => e.GetErrorMessages().Count == 1 &&
                        e.GetErrorMessages().Contains(ResourceMessageException.CPF_ALREADY_EXIST));
        }

        private static RegisterUserUseCase CreateUseCase(string? email = null, string? cpf = null, RefreshToken? tokenRefresh = null)
        {
            var mapper = MappingBuilder.Build();
            var passwordEncrypter = PasswordEncrypterBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var writeRepository = UserWriteOnlyRepositoryBuilder.Build();
            var readRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
            var tokenGenerator = JwtTokenGeneratorBuilder.Build();
            var registerWalletUseCase = RegisterWalletUseCaseBuilder.Build();

            if (!string.IsNullOrEmpty(email))
                readRepositoryBuilder.ExistActiveUserWithEmail(email);
            if (!string.IsNullOrEmpty(cpf))
                readRepositoryBuilder.ExistActiveUserWithCpf(cpf);

            var refreshTokenGen = RefreshTokenGeneratorBuilder.Build();
            var tokenRepo = new TokenRepositoryBuilder();

            if (tokenRefresh != null)
                tokenRepo.Get(tokenRefresh);

            return new RegisterUserUseCase(readRepositoryBuilder.Build(), writeRepository, mapper, passwordEncrypter, unitOfWork, tokenGenerator, registerWalletUseCase, refreshTokenGen, tokenRepo.Build());
        }
    }
}
