using System.Numerics;
using Wallet.Domain.Enum;

namespace Wallet.Domain.Entities
{
    public class Wallet : EntityBase
    {
        private decimal Balance { get; set; } = decimal.Zero;
        private decimal PendingBalance { get; set; } = decimal.Zero;
        private string TransactionPassword { get; set; } = string.Empty;
        private long UserId { get; set; }
        private WalletStatus Status {  get; set; }
    }
}
