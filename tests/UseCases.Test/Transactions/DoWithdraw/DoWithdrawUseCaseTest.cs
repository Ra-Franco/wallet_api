using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Transactions;
using CommonTestUtilities.Repositories.Wallets;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services;
using FluentAssertions;
using Wallet.Application.UseCases.Transaction.Withdraw;
using Wallet.Domain.Entities;
using Wallet.Domain.Enum;
using Wallet.Exceptions;
using Wallet.Exceptions.ExceptionsBase;
using Wallet.Exceptions.TransactionalException;
using Wallet.Exceptions.TransactionalPassword;

namespace UseCases.Test.Transactions.DoWithdraw
{
    public class DoWithdrawUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, var _) = UserBuilder.Build();
            var wallet = WalletBuilder.Build(user);

            var useCase = CreateUseCase(user, wallet, wallet.TransactionPassword);

            var request = RequestCreateWithdrawBuilder.Build();

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
            var request = RequestCreateWithdrawBuilder.Build(characteres: 504);

            var useCase = CreateUseCase(user, wallet, wallet.TransactionPassword);

            Func<Task> act = async () => { await useCase.Execute(request); };

            (await act.Should().ThrowAsync<ErrorOnValidationException>())
                .Where(e => e.GetErrorMessages().Count == 1 &&
                        e.GetErrorMessages().Contains(ResourceMessageException.TRANSACTIONAL_DESCRIPTION_MAX_LENGTH));
        }

        [Fact]
        public async Task Error_Amount_Not_Avaliable()
        {
            (var user, var _) = UserBuilder.Build();
            var wallet = WalletBuilder.Build(user);
            var request = RequestCreateWithdrawBuilder.Build();

            wallet.Balance = 1;
            request.Amount = "2";

            var useCase = CreateUseCase(user, wallet, wallet.TransactionPassword);

            Func<Task> act = async () => { await useCase.Execute(request); };

            (await act.Should().ThrowAsync<AmountNotAvaliableExecption>())
                .Where(e => e.GetErrorMessages().Contains(ResourceMessageException.AMOUNT_NOT_AVALIABLE));
        }

        [Fact]
        public async Task Error_Transactional_Password_Not_Found()
        {
            (var user, var _) = UserBuilder.Build();
            var wallet = WalletBuilder.Build(user);
            var request = RequestCreateWithdrawBuilder.Build();

            var useCase = CreateUseCase(user, wallet);

            Func<Task> act = async () => { await useCase.Execute(request); };

            (await act.Should().ThrowAsync<InvalidTransactionalPassword>())
                .Where(e => e.GetErrorMessages().Contains(ResourceMessageException.TRANSACTIONAL_PASSWORD_INVALID));
        }

        private static DoWithdrawUseCase CreateUseCase(User user, WalletEntity? wallet, string transactionalPassword = "invalid")
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var walletReadRepo = new WalletReadOnlyRepositoryBuilder();

            if (wallet != null)
                walletReadRepo.FindWalletByUserId(wallet);

            var transactionNumberGenerator = TransactionNumberGeneratorBuilder.Build();
            var transactionWriteRepo = TransactionWriteOnlyRepositoryBuilder.Build();
            var walletWriteRepo = WalletWriteOnlyRepositoryBuilder.Build();
            var mapper = MapperBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();

            var transferPasswordValidator = TransferPasswordValidatorBuilder.Build(user.Id, transactionalPassword);

            if (transactionalPassword.Equals("invalid"))
                transferPasswordValidator = TransferPasswordValidatorBuilder.BuildWithInvalidPassword();

            return new DoWithdrawUseCase(loggedUser, walletReadRepo.Build(), transactionNumberGenerator, transactionWriteRepo, walletWriteRepo, mapper, unitOfWork, transferPasswordValidator);
        }
    }
}
