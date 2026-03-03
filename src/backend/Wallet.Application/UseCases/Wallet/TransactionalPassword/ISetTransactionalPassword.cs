using Wallet.Communication.Requests.Wallet;

namespace Wallet.Application.UseCases.Wallet.TransactionalPassword
{
    public interface ISetTransactionalPassword
    {
        public Task Execute(RequestSetTransactionPasswordJson request);
    }
}
