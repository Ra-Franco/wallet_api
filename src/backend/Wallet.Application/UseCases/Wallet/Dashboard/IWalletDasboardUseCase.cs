using Wallet.Communication.Responses.Wallet;

namespace Wallet.Application.UseCases.Wallet.Get
{
    public interface IWalletDasboardUseCase
    {
        public Task<ResponseWalletDashboard> Execute();
    }
}
