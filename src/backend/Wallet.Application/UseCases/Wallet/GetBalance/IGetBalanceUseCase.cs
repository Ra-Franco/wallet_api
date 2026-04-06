using Wallet.Communication.Responses.Wallet;

namespace Wallet.Application.UseCases.Wallet.GetBalance
{
    public interface IGetBalanceUseCase
    {
        public Task<ResponseGetBalance> Execute();
    }
}
