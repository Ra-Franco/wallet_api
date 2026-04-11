using AutoMapper;
using Wallet.Communication.Responses.Wallet;
using Wallet.Domain.Repositories.Wallet;
using Wallet.Domain.Services.LoggedUser;
using Wallet.Exceptions;
using Wallet.Exceptions.ExceptionsBase;
using Wallet.Exceptions.TransactionalPassword;

namespace Wallet.Application.UseCases.Wallet.Dashboard
{
    public class WalletDasboardUseCase : IWalletDasboardUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IWalletReadOnlyRepository _repository;
        private readonly IMapper _mapper;

        public WalletDasboardUseCase(ILoggedUser loggedUser, IWalletReadOnlyRepository repository, IMapper mapper)
        {
            _loggedUser = loggedUser;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ResponseWalletDashboard> Execute()
        {
            var loggedUser = await _loggedUser.User();
            var wallet = await _repository.GetWalletDashboard(loggedUser.Id) ?? throw new NotFoundException(ResourceMessageException.WALLET_NOT_FOUND);

            if (string.IsNullOrEmpty(wallet.TransactionPassword))
                throw new TransactionPasswordNotFound();

            return _mapper.Map<ResponseWalletDashboard>(wallet);
        }
    }
}
