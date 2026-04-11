using Wallet.Domain.Enum;

namespace Wallet.Communication.Requests.User.Security
{
    public class RequestUpdateSecuritySettings
    {
        public TransactionLimitPeriod TransactionLimitPeriod {  get; set; }
        public string TransactionLimit {  get; set; } = string.Empty;
    }
}
