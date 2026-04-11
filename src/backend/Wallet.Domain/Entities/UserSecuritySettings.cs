using System.ComponentModel.DataAnnotations.Schema;
using Wallet.Domain.Enum;

namespace Wallet.Domain.Entities
{
    [Table("users_security_settings")]
    public class UserSecuritySettings : EntityBase
    {
        [Column("user_id")]
        public long UserId { get; set; }
        [Column("transaction_limit_period")]
        public TransactionLimitPeriod TransactionLimitPeriod { get; set; }
        [Column("transaction_limit")]
        public decimal TransactionLimit { get; set;  }
    }
}
