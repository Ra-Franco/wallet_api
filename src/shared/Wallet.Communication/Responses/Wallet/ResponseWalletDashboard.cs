namespace Wallet.Communication.Responses.Wallet
{
    public class ResponseWalletDashboard
    {
        public long Id { get; set; }
        public decimal Balance { get; set; }
        public decimal PendingBalance { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool HasTransactionPassword { get; set; } 
    }
}
