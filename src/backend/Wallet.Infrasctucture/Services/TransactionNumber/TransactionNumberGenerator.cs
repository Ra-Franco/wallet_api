using Wallet.Domain.Services.TransactionNumber;

namespace Wallet.Infrasctructure.Services.TransactionNumber
{
    public class TransactionNumberGenerator : ITransactionNumberGenerator
    {
        public string Generate()
        {
            var guid = Guid.NewGuid().ToString("N");

            return $"TRX{guid}";
        }
    }
}
