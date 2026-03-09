using AutoMapper;
using System.Globalization;
using Wallet.Application.Services.LoggedUser;
using Wallet.Communication.Requests.Deposit;
using Wallet.Communication.Responses.Transaction;
using Wallet.Domain.Enum;
using Wallet.Domain.Repositories;
using Wallet.Domain.Repositories.Transactions;
using Wallet.Domain.Repositories.Wallet;
using Wallet.Domain.Services.TransactionNumber;
using Wallet.Exceptions;
using Wallet.Exceptions.ExceptionsBase;

namespace Wallet.Application.UseCases.Transaction.Deposits
{
    public class CreateDepositUseCase : ICreateDepositUseCase
    {
        private readonly ITransactionWriteOnlyRepository _transactionWriteRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggedUser _loggedUser;
        private readonly ITransactionNumberGenerator _numberGenerator;
        private readonly IWalletReadOnlyRepository _walletReadRepo;
        private readonly IWalletWriteOnlyRepository _walletWriteRepo;
        private readonly IMapper _mapper;

        public CreateDepositUseCase(ITransactionWriteOnlyRepository transactionWriteRepo, IUnitOfWork unitOfWork, ILoggedUser loggedUser, ITransactionNumberGenerator numberGenerator, IWalletReadOnlyRepository walletReadRepo, IWalletWriteOnlyRepository walletWriteRepo = null, IMapper mapper = null)
        {
            _transactionWriteRepo = transactionWriteRepo;
            _unitOfWork = unitOfWork;
            _loggedUser = loggedUser;
            _numberGenerator = numberGenerator;
            _walletReadRepo = walletReadRepo;
            _walletWriteRepo = walletWriteRepo;
            _mapper = mapper;
        }

        public async Task<ResponseTransaction> Execute(RequestCreateDeposit request)
        {
            await Validate(request);
            var loggedUser = await _loggedUser.User();
            var wallet = await _walletReadRepo.FindWalletByUserId(loggedUser.Id);
            var amount = decimal.Parse(request.Amount, CultureInfo.CurrentCulture);

            var transaction = new Domain.Entities.Transaction
            {
                TransactionNumber = _numberGenerator.Generate(),
                WalletId = wallet.Id,
                Type = TransactionType.Deposit,
                Amount = amount,
                Description = request.Description,
                TransactionDate = DateTime.UtcNow,
                Status = TransactionStatus.Completed
            };
            wallet.Balance = amount + wallet.Balance;

            await _transactionWriteRepo.Add(transaction);
            await _walletWriteRepo.Update(wallet);
            await _unitOfWork.Commit();

            var response = _mapper.Map<ResponseTransaction>(transaction);
            response.Amount = amount;

            return response;
        }

        public async Task Validate(RequestCreateDeposit request)
        {
            var validator = new CreateDepositValidator();
            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
