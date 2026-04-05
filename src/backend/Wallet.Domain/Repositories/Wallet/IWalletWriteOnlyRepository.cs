using System.Linq.Expressions;
using Wallet.Domain.Entities;

namespace Wallet.Domain.Repositories.Wallet
{
    public interface IWalletWriteOnlyRepository
    {
        public Task CreateWallet(Entities.WalletEntity wallet);

        public Task UpdateTransactionPassword(string transactionPassword, long walletId);
        public Task UpdateAmount(long walletId, decimal balance);
    }
}
