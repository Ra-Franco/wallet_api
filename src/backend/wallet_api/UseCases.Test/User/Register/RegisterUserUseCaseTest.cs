using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using Wallet.Application.UseCases.User.Register;
using Wallet.Exceptions;
using Wallet.Exceptions.ExceptionsBase;

namespace UseCases.Test.User.Register
{
    public class RegisterUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            var useCase = CreateUseCase();

            var result = await useCase.Execute(request);

            result.Should().NotBeNull();
            result.Name.Should().Be(request.Name);
        }

        [Fact]
        public async Task Error_Email_Already_Registered()
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            var useCase = CreateUseCase(request.Email);

            Func<Task> act = async () => await useCase.Execute(request);
            (await act.Should().ThrowAsync<ErrorOnValidationException>())
                .Where(e => e.ErrorMessages.Count == 1 &&
                        e.ErrorMessages.Contains(ResourceMessageException.EMAIL_ALREADY_REGISTERED));
        }
        [Fact]
        public async Task Cpf_Email_Already_Registered()
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            var useCase = CreateUseCase(string.Empty, request.CPF);

            Func<Task> act = async () => await useCase.Execute(request);
            (await act.Should().ThrowAsync<ErrorOnValidationException>())
                .Where(e => e.ErrorMessages.Count == 1 &&
                        e.ErrorMessages.Contains(ResourceMessageException.CPF_ALREADY_EXIST));
        }

        private static RegisterUserUseCase CreateUseCase(string? email = null, string? cpf = null)
        {
            var mapper = MappingBuilder.Build();
            var passwordEncrypter = PasswordEncrypterBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var writeRepository = UserWriteOnlyRepositoryBuilder.Build();
            var readRepositoryBuilder = new UserReadOnlyRepositoryBuilder();

            if (!string.IsNullOrEmpty(email))
                readRepositoryBuilder.ExistActiveUserWithEmail(email);
            if (!string.IsNullOrEmpty(cpf))
                readRepositoryBuilder.ExistActiveUserWithCpf(cpf);

            return new RegisterUserUseCase(readRepositoryBuilder.Build(), writeRepository, mapper, passwordEncrypter, unitOfWork);
        }
    }
}
