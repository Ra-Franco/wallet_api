using Wallet.Domain.Enum;

namespace Wallet.Communication.Requests.Transactions
{
    public class RequestTransactionsFilter
    {
        public DateTime? StartDate { get; set; } = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        public DateTime? EndDate { get; set; } = DateTime.UtcNow;
        public IList<TransactionType>? Type { get; set; } = [];
        public IList<TransactionStatus>? Status { get; set; } = [];
    }
}
