using Azure.Core;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.Wallets;
using CommonTestUtilities.Services;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Wallet.Application.UseCases.Wallet.Get;
using Wallet.Domain.Entities;
using Wallet.Exceptions;
using Wallet.Exceptions.ExceptionsBase;
using Wallet.Exceptions.TransactionalPassword;

namespace UseCases.Test.Wallets.Dashboard
{
    public class WalletDasboardUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, var _) = UserBuilder.Build();
            var wallet = WalletBuilder.Build(user);

            var useCase = CreateUseCase(user, wallet);

            var response = await useCase.Execute();

            response.Should().NotBeNull();
            response.Id.Should().Be(wallet.Id);
            response.HasTransactionPassword.Should().BeTrue();
        }

        [Fact]
        public async Task Error_Wallet_Not_Found()
        {
            (var user, var _) = UserBuilder.Build();

            var wallet = WalletBuilder.Build(user);
            user.Id = 9999;
            var useCase = CreateUseCase(user, wallet);

            Func<Task> act = async () => await useCase.Execute();

            (await act.Should().ThrowAsync<NotFoundException>())
                .Where(e => e.GetErrorMessages().Count == 1 &&
                        e.GetErrorMessages().Contains(ResourceMessageException.WALLET_NOT_FOUND));
        }

        [Fact]
        public async Task Error_Transactional_Password_Not_Found()
        {
            (var user, var _) = UserBuilder.Build();

            var wallet = WalletBuilder.Build(user);
            wallet.TransactionPassword = string.Empty;
            var useCase = CreateUseCase(user, wallet);

            Func<Task> act = async () => await useCase.Execute();

            (await act.Should().ThrowAsync<TransactionPasswordNotFound>())
                .Where(e => e.GetErrorMessages().Count == 1 &&
                        e.GetErrorMessages().Contains(ResourceMessageException.TRANSACTIONAL_PASSWORD_IS_NULL));
        }

        private WalletDasboardUseCase CreateUseCase(User user, WalletEntity? wallet)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var repository = new WalletReadOnlyRepositoryBuilder();
            var mapper = MapperBuilder.Build();

            if (wallet != null)
                repository.GetWalletDashboard(wallet);

            return new WalletDasboardUseCase(loggedUser, repository.Build(), mapper);
        }
    }
}
