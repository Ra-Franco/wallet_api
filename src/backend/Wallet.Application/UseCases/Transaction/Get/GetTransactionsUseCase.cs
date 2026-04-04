using AutoMapper;
using Wallet.Application.Services.LoggedUser;
using Wallet.Communication.Requests.Transactions;
using Wallet.Communication.Responses.Transaction;
using Wallet.Domain.Dtos;
using Wallet.Domain.Repositories.Transactions;
using Wallet.Domain.Utils.Page;

namespace Wallet.Application.UseCases.Transaction.Get
{
    public class GetTransactionsUseCase : IGetTransactionsUseCase
    {
        private readonly ITransactionReadOnlyRepository _readRepository;
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;

        public GetTransactionsUseCase(ITransactionReadOnlyRepository readRepository, IMapper mapper, ILoggedUser loggedUser)
        {
            _readRepository = readRepository;
            _mapper = mapper;
            _loggedUser = loggedUser;
        }

        public async Task<PagedList<ResponseTransaction>> Execute(RequestTransactionsFilter request, PageParameters pageParameters)
        {
            var user = await _loggedUser.User();

            var filters = new FilterTransactionsDto
            {
                EndDate = request.EndDate,
                StartDate = request.StartDate,
                Type = request.Type is not null ? [.. request.Type.Distinct()] : [],
                Status = request.Status is not null ? [.. request.Status.Distinct()] : [],
            };

            var transactions = await _readRepository.GetTransactionsByUserId(user.Id, filters, pageParameters);

            return _mapper.Map<PagedList<ResponseTransaction>>(transactions);
        }
    }
}
