using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using Wallet.Domain.Enum;

namespace Wallet.Domain.Entities
{
    public class WalletEntity : EntityBase
    {
        public decimal Balance { get; set; } = decimal.Zero;
        [Column("Pending_Balance")]
        public decimal PendingBalance { get; set; } = decimal.Zero;
        [Column("Transaction_Password")]
        public string TransactionPassword { get; set; } = string.Empty;
        [Column("User_Id")]
        public long UserId { get; set; }
        public WalletStatus Status {  get; set; }
    }
}
