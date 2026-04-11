using Wallet.Communication.Responses.Wallet;

namespace Wallet.Application.UseCases.Wallet.Dashboard
{
    public interface IWalletDasboardUseCase
    {
        public Task<ResponseWalletDashboard> Execute();
    }
}
