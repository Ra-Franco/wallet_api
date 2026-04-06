using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.Wallets;
using CommonTestUtilities.Services;
using FluentAssertions;
using Wallet.Application.UseCases.Wallet.GetBalance;
using Wallet.Domain.Entities;

namespace UseCases.Test.Wallets.GetBalance
{
    public class GetBalanceUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, var _) = UserBuilder.Build();
            var wallet = WalletBuilder.Build(user);
            var useCase = CreateUseCase(user, wallet);

            var response = await useCase.Execute();

            response.Should().NotBeNull();
            response.Balance.Should().BeGreaterThan(-1);
        }

        private static GetBalanceUseCase CreateUseCase(User user, WalletEntity? wallet = null)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var walletReadRepo = new WalletReadOnlyRepositoryBuilder();

            if (wallet != null)
                walletReadRepo.FindBalanceByUserId(wallet);

            return new GetBalanceUseCase(loggedUser, walletReadRepo.Build());
        }
    }
}
