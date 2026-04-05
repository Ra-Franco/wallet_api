namespace Wallet.Communication.Responses.Transaction
{
    public class ResponseShortTransaction
    {
        public string TransactionNumber { get; set; } = string.Empty;
        public string TransactionType { get; set; } = string.Empty;
        public decimal Amount { get; set; } = 0;
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
