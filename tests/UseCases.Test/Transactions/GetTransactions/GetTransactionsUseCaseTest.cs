using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.Transactions;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Requests.Page;
using CommonTestUtilities.Services;
using FluentAssertions;
using Wallet.Application.UseCases.Transaction.Get;
using Wallet.Domain.Entities;
using Wallet.Domain.Utils.Page;

namespace UseCases.Test.Transactions.GetTransactions
{
    public class GetTransactionsUseCaseTest
    {

        [Fact]
        public async Task Success()
        {
            (var user, var _) = UserBuilder.Build();
            var wallet = WalletBuilder.Build(user);
            var transactions = TransactionBuilder.BuildList(wallet.Id, 10);
            var pagedList = PagedListBuilder.Build(transactions);

            var useCase = CreateUseCase(user, pagedList);

            var request = RequestTransactionsFilterBuilder.Build();
            var pageParameters = new PageParameters();

            var result = await useCase.Execute(request, pageParameters);

            result.Should().NotBeNull();
            result.Page.Should().NotBe(0);
            result.PageSize.Should().NotBe(0);
            result.Items.Should().NotBeNull();
            result.Items.Should().HaveCount(transactions.Count);
        }

        private static GetTransactionsUseCase CreateUseCase(User user, PagedList<Transaction>? pagedList)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var mapper = MapperBuilder.Build();
            var repository = new TransactionReadOnlyRepositoryBuilder();

            if (pagedList != null)
                repository.GetTransactionsByUserId(user.Id, pagedList);

            return new GetTransactionsUseCase(repository.Build(), mapper, loggedUser);
        }   
    }
}
