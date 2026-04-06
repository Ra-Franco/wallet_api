using System.ComponentModel.DataAnnotations.Schema;

namespace Wallet.Domain.Entities
{
    [Table("refresh_token")]
    public class RefreshToken : EntityBase
    {
        public required string Value { get; set; } = string.Empty;
        [Column("user_id")]
        public required long UserId { get; set; }
        public User User { get; set; } = default!;
    }
}
