namespace Wallet.Communication.Responses.Transaction
{
    public class ResponseTransfer
    {
        public string TransactionNumber { get; set; } = string.Empty;
        public decimal Amount { get; set; } = decimal.Zero;
        public string SenderName { get; set; } = string.Empty;
        public string ReceiverCpf { get; set; } = string.Empty;
        public string Description { get; set; }  = string.Empty;
        public DateTime Date { get; set; }  = DateTime.MinValue;
        public string Status { get; set; }   = string.Empty;
    }
}
