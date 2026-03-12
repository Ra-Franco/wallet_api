using Wallet.Domain.Enum;

namespace Wallet.Domain.Dtos
{
    public class FilterTransactionsDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public IList<TransactionType>? Type { get; set; }
        public IList<TransactionStatus>? Status { get; set; }
    }
}
