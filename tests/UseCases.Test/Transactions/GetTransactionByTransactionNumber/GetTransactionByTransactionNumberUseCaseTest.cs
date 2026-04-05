using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.Transactions;
using CommonTestUtilities.Repositories.Wallets;
using CommonTestUtilities.Services;
using FluentAssertions;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using Wallet.Application.UseCases.Transaction.GetById;
using Wallet.Domain.Entities;

namespace UseCases.Test.Transactions.GetTransactionById
{
    public class GetTransactionByTransactionNumberUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, var _) = UserBuilder.Build();
            var wallet = WalletBuilder.Build(user);
            var transaction = TransactionBuilder.Build(wallet.Id);

            var useCase = CreateUseCase(user, wallet, transaction);
            var response = await useCase.Execute(transaction.TransactionNumber);

            response.Should().NotBeNull();
            response.TransactionNumber.Should().Be(transaction.TransactionNumber);
            response.Amount.Should().Be(transaction.Amount);
            response.Status.Should().Be(transaction.Status.ToString());
        }

        [Fact]
        public async Task Different_Wallet_Id()
        {
            (var user, var _) = UserBuilder.Build();
            var wallet = WalletBuilder.Build(user);
            var transaction = TransactionBuilder.Build(wallet.Id + 1);

            var useCase = CreateUseCase(user, wallet, transaction);
            var response = await useCase.Execute(transaction.TransactionNumber);

            response.Should().BeNull();
        }

        [Fact]
        public async Task Success_Related_Wallet_Id()
        {
            (var user, var _) = UserBuilder.Build();
            var wallet = WalletBuilder.Build(user);
            var transaction = TransactionBuilder.Build(wallet.Id +1);
            transaction.RelatedWalletId = wallet.Id;

            var useCase = CreateUseCase(user, wallet, transaction);
            var response = await useCase.Execute(transaction.TransactionNumber);

            response.Should().NotBeNull();
            response.TransactionNumber.Should().Be(transaction.TransactionNumber);
            response.Amount.Should().Be(transaction.Amount);
            response.Status.Should().Be(transaction.Status.ToString());
            response.RelatedWalletId.Should().Be(wallet.Id);
        }

        private static GetTransactionByTransactionNumberUseCase CreateUseCase(User user, WalletEntity wallet, Transaction transaction)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var transactionReadRepo = new TransactionReadOnlyRepositoryBuilder();

            if (transaction.WalletId == wallet.Id || wallet.Id == transaction.RelatedWalletId)
                transactionReadRepo.GetByTransactionNumber(transaction.TransactionNumber, wallet.Id, transaction);

            var mapper = MapperBuilder.Build();
            var walletReadRepo = new WalletReadOnlyRepositoryBuilder();
            walletReadRepo.FindWalletByUserId(wallet);

            return new GetTransactionByTransactionNumberUseCase(loggedUser, transactionReadRepo.Build(),mapper, walletReadRepo.Build());
        }
    }
}
