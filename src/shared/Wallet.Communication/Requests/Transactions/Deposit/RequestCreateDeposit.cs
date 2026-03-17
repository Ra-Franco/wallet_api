namespace Wallet.Communication.Requests.Transactions.Deposit
{
    public class RequestCreateDeposit
    {
        public string Description { get; set; } = string.Empty;
        public string Amount { get; set; } = string.Empty;
    }
}
