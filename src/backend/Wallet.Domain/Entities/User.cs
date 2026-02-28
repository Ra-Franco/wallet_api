using System.ComponentModel.DataAnnotations.Schema;
using Wallet.Domain.Enum;

namespace Wallet.Domain.Entities
{
    public class User : EntityBase
    {
        public string Name { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        [Column("birth_date")]
        public DateTime BirthDate { get; set; }
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Phonenumber { get; set; } = string.Empty;
        public string Occupation { get; set; } = string.Empty;
        public decimal Income { get; set; } = decimal.Zero;
        public UserStatus Status { get; set; }
        public Guid UserIdentifier { get; set; }
    }
}
