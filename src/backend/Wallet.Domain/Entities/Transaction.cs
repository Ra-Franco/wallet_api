using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;
using Wallet.Domain.Enum;

namespace Wallet.Domain.Entities
{
    public class Transaction : EntityBase
    {
        [Column("transaction_number")]
        public string TransactionNumber { get; set; } = string.Empty;
        [Column("wallet_id")]
        public long WalletId { get; set; }
        public WalletEntity Wallet { get; set; }
        public TransactionType Type { get; set; } 
        public decimal Amount { get; set; }
        [Column("related_wallet_id")]
        public long? RelatedWalletId { get; set; }
        public WalletEntity? RelatedWallet { get; set; }
        public string Description { get; set; } = string.Empty;
        [Column("transaction_date")]
        public DateTime TransactionDate { get; set; }
        public Enum.TransactionStatus Status { get; set; }
    }
}
