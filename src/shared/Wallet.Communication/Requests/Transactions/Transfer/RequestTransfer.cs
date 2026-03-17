namespace Wallet.Communication.Requests.Transactions.Transfer
{
    public class RequestTransfer
    {
        public string ReceiverCpf { get; set; } = string.Empty;
        public decimal Amount { get; set; } = decimal.Zero;
        public string Description { get; set;  } = string.Empty;
        public string TransactionPassword { get; set; } = string.Empty;
    }
}
