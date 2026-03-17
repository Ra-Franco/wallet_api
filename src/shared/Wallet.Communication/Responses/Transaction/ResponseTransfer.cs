namespace Wallet.Communication.Responses.Transaction
{
    public class ResponseTransfer
    {
        public string TransactionNumber { get; set; }
        public decimal Amount { get; set; }
        public string SenderName { get; set; }
        public string ReceiverCpf { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }
}
