using Moq;
using Wallet.Domain.Dtos;
using Wallet.Domain.Entities;
using Wallet.Domain.Repositories.Transactions;
using Wallet.Domain.Utils.Page;

namespace CommonTestUtilities.Repositories.Transactions
{
    public class TransactionReadOnlyRepositoryBuilder
    {
        private readonly Mock<ITransactionReadOnlyRepository> _repository;

        public TransactionReadOnlyRepositoryBuilder() => _repository = new Mock<ITransactionReadOnlyRepository>();

        public ITransactionReadOnlyRepository Build() => _repository.Object;

        public void GetTransactionsByUserId(long id, PagedList<Transaction> transactionList)
        {
            _repository.Setup(repo => repo.GetTransactionsByUserId(id, It.IsAny<FilterTransactionsDto>(), It.IsAny<PageParameters>())).ReturnsAsync(transactionList);
        }

        public void GetByTransactionNumber(string transactionNumber, long walletId, Transaction transaction)
        {
            _repository.Setup(repo => repo.GetByTransactionNumber(transactionNumber, walletId)).ReturnsAsync(transaction);
        }
    }
}
