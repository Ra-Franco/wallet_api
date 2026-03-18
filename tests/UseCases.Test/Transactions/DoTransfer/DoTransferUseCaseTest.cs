using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Transactions;
using CommonTestUtilities.Repositories.Wallets;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services;
using FluentAssertions;
using Wallet.Application.UseCases.Transaction.Transfer;
using Wallet.Domain.Entities;
using Wallet.Domain.Enum;
using Wallet.Exceptions;
using Wallet.Exceptions.ExceptionsBase;
using Wallet.Exceptions.TransactionalException;
using Wallet.Exceptions.TransactionalPassword;

namespace UseCases.Test.Transactions.DoTransfer
{
    public class DoTransferUseCaseTest
    {

        [Fact]
        public async Task Success()
        {
            (var user, var _) = UserBuilder.Build();
            var wallet = WalletBuilder.Build(user);
            var receiverWallet = WalletBuilder.Build(user);
            var request = RequestTransferBuilder.Build();

            var useCase = CreateUseCase(user, wallet, receiverWallet, request.ReceiverCpf, wallet.TransactionPassword);

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
            var receiverWallet = WalletBuilder.Build(user);
            var request = RequestTransferBuilder.Build(characteres: 504);

            var useCase = CreateUseCase(user, wallet, receiverWallet, request.ReceiverCpf, wallet.TransactionPassword);

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
            var receiverWallet = WalletBuilder.Build(user);
            var request = RequestTransferBuilder.Build();

            wallet.Balance = 1;
            request.Amount = 2;

            var useCase = CreateUseCase(user, wallet, receiverWallet, request.ReceiverCpf, wallet.TransactionPassword);

            Func<Task> act = async () => { await useCase.Execute(request); };

            (await act.Should().ThrowAsync<AmountNotAvaliableExecption>())
                .Where(e => e.GetErrorMessages().Contains(ResourceMessageException.AMOUNT_NOT_AVALIABLE));
        }

        [Fact]
        public async Task Error_Receiver_Wallet_Not_Found()
        {
            (var user, var _) = UserBuilder.Build();
            var wallet = WalletBuilder.Build(user);
            var receiverWallet = WalletBuilder.Build(user);
            var request = RequestTransferBuilder.Build();

            receiverWallet = null;

            var useCase = CreateUseCase(user, wallet, receiverWallet, request.ReceiverCpf, wallet.TransactionPassword);

            Func<Task> act = async () => { await useCase.Execute(request); };

            (await act.Should().ThrowAsync<NotFoundException>())
                .Where(e => e.GetErrorMessages().Contains(ResourceMessageException.RECEIVER_WALLET_NOT_FOUND));
        }

        [Fact]
        public async Task Error_Transactional_Password_Not_Found()
        {
            (var user, var _) = UserBuilder.Build();
            var wallet = WalletBuilder.Build(user);
            var receiverWallet = WalletBuilder.Build(user);
            var request = RequestTransferBuilder.Build();

            var useCase = CreateUseCase(user, wallet, receiverWallet, request.ReceiverCpf);

            Func<Task> act = async () => { await useCase.Execute(request); };

            (await act.Should().ThrowAsync<InvalidTransactionalPassword>())
                .Where(e => e.GetErrorMessages().Contains(ResourceMessageException.TRANSACTIONAL_PASSWORD_INVALID));
        }

        private static DoTransferUseCase CreateUseCase(User user, WalletEntity? wallet, WalletEntity? receiverWallet, string receiverCpf, string transactionalPassword = "invalid")
        {
            var loggedUser = LoggedUserBuilder.Build(user);

            var transferPasswordValidator = TransferPasswordValidatorBuilder.Build(user.Id, transactionalPassword);

            if (transactionalPassword.Equals("invalid"))
                transferPasswordValidator = TransferPasswordValidatorBuilder.BuildWithInvalidPassword();

            var transactionWriteRepo = TransactionWriteOnlyRepositoryBuilder.Build();

            var walletReadRepo = new WalletReadOnlyRepositoryBuilder();

            if (wallet != null)
                walletReadRepo.FindWalletByUserId(wallet);

            if (receiverWallet != null && !string.IsNullOrEmpty(receiverCpf))
                walletReadRepo.FindWalletByCpf(receiverCpf, receiverWallet);

            var transactionNumberGenerator = TransactionNumberGeneratorBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var walletWriteRepo = WalletWriteOnlyRepositoryBuilder.Build();

            return new DoTransferUseCase(loggedUser, transferPasswordValidator, transactionWriteRepo, walletReadRepo.Build(), transactionNumberGenerator, unitOfWork, walletWriteRepo);
        }
    }
}
