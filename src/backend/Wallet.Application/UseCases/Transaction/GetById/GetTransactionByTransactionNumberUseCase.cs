using AutoMapper;
using Wallet.Communication.Responses.Transaction;
using Wallet.Domain.Repositories.Transactions;
using Wallet.Domain.Repositories.Wallet;
using Wallet.Domain.Services.LoggedUser;

namespace Wallet.Application.UseCases.Transaction.GetById
{
    public class GetTransactionByTransactionNumberUseCase : IGetTransactionByTransactionNumberUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly ITransactionReadOnlyRepository _transactionReadRepository;
        private readonly IWalletReadOnlyRepository _walletReadRepository;
        private readonly IMapper _mapper;

        public GetTransactionByTransactionNumberUseCase(ILoggedUser loggedUser, ITransactionReadOnlyRepository transactionReadRepository, IMapper mapper, IWalletReadOnlyRepository walletReadRepository)
        {
            _loggedUser = loggedUser;
            _transactionReadRepository = transactionReadRepository;
            _mapper = mapper;
            _walletReadRepository = walletReadRepository;
        }

        public async Task<ResponseTransaction> Execute(string transactionNumber)
        {
            var loggedUser = await _loggedUser.User();
            var wallet = await _walletReadRepository.FindWalletByUserId(loggedUser.Id);

            var transaction = await _transactionReadRepository.GetByTransactionNumber(transactionNumber, wallet.Id);

            return _mapper.Map<ResponseTransaction>(transaction);
        }
    }
}
