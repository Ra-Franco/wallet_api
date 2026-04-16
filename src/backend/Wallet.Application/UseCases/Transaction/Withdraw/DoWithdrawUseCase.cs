using AutoMapper;
using Wallet.Communication.Requests.Transactions.Withdraw;
using Wallet.Communication.Responses.Transaction;
using Wallet.Communication.Utils;
using Wallet.Domain.Repositories;
using Wallet.Domain.Repositories.Transactions;
using Wallet.Domain.Repositories.Wallet;
using Wallet.Domain.Security.TransferPassword;
using Wallet.Domain.Services.LoggedUser;
using Wallet.Domain.Services.Transactional;
using Wallet.Domain.Services.TransactionNumber;
using Wallet.Exceptions.ExceptionsBase;
using Wallet.Exceptions.TransactionalException;

namespace Wallet.Application.UseCases.Transaction.Withdraw
{
    public class DoWithdrawUseCase : IDoWithdrawUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IWalletReadOnlyRepository _walletReadRepository;
        private readonly ITransactionNumberGenerator _transactionNumberGenerator;
        private readonly ITransactionWriteOnlyRepository _transactionWriteRepository;
        private readonly IWalletWriteOnlyRepository _walletWriteRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITransferPasswordValidator _transferPasswordValidator;
        private readonly IUserSecurityTransactional _userSecurityTransactional;

        public DoWithdrawUseCase(ILoggedUser loggedUser, IWalletReadOnlyRepository walletReadRepository, ITransactionNumberGenerator transactionNumberGenerator, ITransactionWriteOnlyRepository transactionWriteRepository, IWalletWriteOnlyRepository walletWriteRepository, IMapper mapper, IUnitOfWork unitOfWork, ITransferPasswordValidator transferPasswordValidator, IUserSecurityTransactional userSecurityTransactional)
        {
            _loggedUser = loggedUser;
            _walletReadRepository = walletReadRepository;
            _transactionNumberGenerator = transactionNumberGenerator;
            _transactionWriteRepository = transactionWriteRepository;
            _walletWriteRepository = walletWriteRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _transferPasswordValidator = transferPasswordValidator;
            _userSecurityTransactional = userSecurityTransactional;
        }

        public async Task<ResponseShortTransaction> Execute(RequestCreateWithdraw request)
        {
            Validate(request);
            var user = await _loggedUser.User();
            var wallet = await _walletReadRepository.FindWalletByUserId(user.Id);
            
            await _transferPasswordValidator.Validate(user.Id, request.TransactionPassword);
            
            var amount = request.Amount.StringToDecimalCurrency();
            await _userSecurityTransactional.ValidateTransactionalSecurity(amount,user.Id);
            
            if (wallet.Balance < amount)
                throw new AmountNotAvaliableExecption();

            var transaction = new Domain.Entities.Transaction
            {
                WalletId = wallet.Id,
                Amount = amount,
                Description = request.Description,
                Type = Domain.Enum.TransactionType.Withdraw,
                Status = Domain.Enum.TransactionStatus.Completed,
                TransactionNumber = _transactionNumberGenerator.Generate(),
                TransactionDate = DateTime.UtcNow
            };
            await _transactionWriteRepository.Add(transaction);

            wallet.Balance = wallet.Balance - amount;

            await _walletWriteRepository.UpdateAmount(wallet.Id, wallet.Balance);
            await _unitOfWork.Commit();

            var response = _mapper.Map<ResponseShortTransaction>(transaction);
            response.Amount = amount;

            return response;
        }

        public void Validate(RequestCreateWithdraw request)
        {
            var validator = new DoWithdrawValidator();
            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
