using System.ComponentModel;

namespace Wallet.Communication.Requests.Transactions.Withdraw
{
    public class RequestCreateWithdraw
    {
        public required string Amount { get; set; }
        public string? Description { get; set; }
        public string TransactionPassword { get; set; } = string.Empty;
    }
}
