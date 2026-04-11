using Wallet.Domain.Enum;
using Wallet.Domain.Repositories;
using Wallet.Domain.Repositories.Wallet;

namespace Wallet.Application.UseCases.Wallet.Register
{
    public class RegisterWalletUseCase : IRegisterWalletUseCase
    {
        private readonly IWalletWriteOnlyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterWalletUseCase(IWalletWriteOnlyRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(Domain.Entities.User user)
        {
            var wallet = new Domain.Entities.WalletEntity
            {
                UserId = user.Id,
                Status = WalletStatus.Active
            };

            await _repository.CreateWallet(wallet);
            await _unitOfWork.Commit();
        }
    }
}
