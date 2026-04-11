namespace Wallet.Communication.Responses.Transaction
{
    public class ResponseTransaction
    {
        public string TransactionNumber { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public long? RelatedWalletId { get; set; }
    }
}
