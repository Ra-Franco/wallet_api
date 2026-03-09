using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Transactions;
using CommonTestUtilities.Repositories.Wallets;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services;
using FluentAssertions;
using Wallet.Application.UseCases.Transaction.Deposits;
using Wallet.Domain.Entities;
using Wallet.Domain.Enum;
using Wallet.Exceptions;
using Wallet.Exceptions.ExceptionsBase;

namespace UseCases.Test.Transactions.Deposit
{
    public class CreateDepositUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, var _) = UserBuilder.Build();
            var wallet = WalletBuilder.Build(user);
            var useCase = CreateUseCase(user, wallet);
            var request = RequestCreateDepositBuilder.Build();

            var response = await useCase.Execute(request);

            response.TransactionNumber.Should().NotBeNullOrEmpty();
            response.Amount.Should().BeGreaterThan(0);
            response.Status.Should().Be(TransactionStatus.Completed.ToString());
        }

        [Fact]
        public async Task Error_Validator()
        {
            (var user, var _) = UserBuilder.Build();
            var wallet = WalletBuilder.Build(user);
            var useCase = CreateUseCase(user, wallet);
            var request = RequestCreateDepositBuilder.Build(500);

            Func<Task> act = async () => { await useCase.Execute(request); };

            (await act.Should().ThrowAsync<ErrorOnValidationException>())
                .Where(e => e.GetErrorMessages().Count == 1 &&
                        e.GetErrorMessages().Contains(ResourceMessageException.TRANSACTIONAL_DESCRIPTION_MAX_LENGTH));
        }

        private CreateDepositUseCase CreateUseCase(User user, WalletEntity wallet)
        {
            var transactionWriteRepo = TransactionWriteOnlyRepositoryBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);
            var numberGenerator = TransactionNumberGeneratorBuilder.Build();
            var walletReadRepo = new WalletReadOnlyRepositoryBuilder(); 
            var walletWriteRepo = WalletWriteOnlyRepositoryBuilder.Build();
            var mapper = MapperBuilder.Build();

            if (wallet != null)
                walletReadRepo.FindWalletByUserId(wallet);

            return new CreateDepositUseCase(transactionWriteRepo, unitOfWork, loggedUser, numberGenerator, walletReadRepo.Build(), walletWriteRepo, mapper);
        }
    }
}