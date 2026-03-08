using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Repositories.Transactions;
using Wallet.Domain.Services.TransactionNumber;

namespace Wallet.Infrasctructure.Services.TransactionNumber
{
    public class TransactionNumberGenerator : ITransactionNumberGenerator
    {
        private readonly ITransactionReadOnlyRepository _repository;
        public string Generate()
        {
            var guid = Guid.NewGuid().ToString("N");

            return $"TRX{guid}";
        }
    }
}
