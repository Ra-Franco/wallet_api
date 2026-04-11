using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Wallets;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services;
using FluentAssertions;
using Wallet.Application.UseCases.Wallet.TransactionalPassword;
using Wallet.Domain.Entities;
using Wallet.Exceptions;

namespace UseCases.Test.Wallets.TransactionalPassword
{
    public class SetTransactionalPasswordTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();
            var wallet = WalletBuilder.Build(user);
            var useCase = CreateUseCase(user, wallet);

            var request = RequestSetTransactionPasswordJsonBuilder.Build();

            Func<Task> act = async () => await useCase.Execute(request);

            await act.Should().NotThrowAsync();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void Error_Password_Invalid(int passwordLength)
        {
            var validator = new SetTransactionalPasswordValidator();
            var request = RequestSetTransactionPasswordJsonBuilder.Build(passwordLength);

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(e => e.ErrorMessage.Equals(ResourceMessageException.TRANSACTIONAL_PASSWORD_LENGTH));
        }

        private SetTransactionalPassword CreateUseCase(User user, WalletEntity? wallet)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var writeRepository = WalletWriteOnlyRepositoryBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var readRepository = new WalletReadOnlyRepositoryBuilder();
            var passwordEncrypter = PasswordEncrypterBuilder.Build();

            if (wallet != null)
                readRepository.FindWalletByUserId(wallet);

            return new SetTransactionalPassword(
                loggedUser, writeRepository, unitOfWork, readRepository.Build(), passwordEncrypter
                );
        }
    }
}
