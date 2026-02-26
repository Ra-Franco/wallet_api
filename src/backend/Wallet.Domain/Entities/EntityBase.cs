namespace Wallet.Domain.Entities
{
    public class EntityBase
    {
        public long Id { get; set; }
        public DateTime Created_On {  get; set; } = DateTime.UtcNow;
        public string Status {  get; set; } = string.Empty;
        public DateTime Last_Updated {  get; set; }
    }
}
