using Wallet.Application.Services.LoggedUser;
using Wallet.Communication.Responses.Wallet;
using Wallet.Domain.Repositories.Wallet;

namespace Wallet.Application.UseCases.Wallet.GetBalance
{
    public class GetBalanceUseCase : IGetBalanceUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IWalletReadOnlyRepository _walletReadRepo;

        public GetBalanceUseCase(ILoggedUser loggedUser, IWalletReadOnlyRepository walletReadRepo)
        {
            _loggedUser = loggedUser;
            _walletReadRepo = walletReadRepo;
        }

        public async Task<ResponseGetBalance> Execute()
        {
            var loggedUser = await _loggedUser.User();
            var balance = await _walletReadRepo.FindBalanceByUserId(loggedUser.Id);

            return new ResponseGetBalance { Balance = balance };
        }
    }
}
