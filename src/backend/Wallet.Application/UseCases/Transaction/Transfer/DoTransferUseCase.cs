using AutoMapper;
using System.Reflection;
using Wallet.Application.Services.LoggedUser;
using Wallet.Communication.Requests.Transactions.Transfer;
using Wallet.Communication.Responses.Transaction;
using Wallet.Domain.Entities;
using Wallet.Domain.Enum;
using Wallet.Domain.Repositories;
using Wallet.Domain.Repositories.Transactions;
using Wallet.Domain.Repositories.Wallet;
using Wallet.Domain.Security.TransferPassword;
using Wallet.Domain.Services.TransactionNumber;
using Wallet.Exceptions;
using Wallet.Exceptions.ExceptionsBase;
using Wallet.Exceptions.TransactionalException;
using Wallet.Exceptions.Wallet;

namespace Wallet.Application.UseCases.Transaction.Transfer
{
    public class DoTransferUseCase : IDoTransferUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly ITransferPasswordValidator _transferPasswordValidator;
        private readonly ITransactionWriteOnlyRepository _transactionWriteRepo;
        private readonly IWalletReadOnlyRepository _walletReadRepo;
        private readonly ITransactionNumberGenerator _transactionNumberGenerator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWalletWriteOnlyRepository _walletWriteRepo;

        public DoTransferUseCase(ILoggedUser loggedUser, ITransferPasswordValidator transferPasswordValidator, ITransactionWriteOnlyRepository transactionWriteRepo, IWalletReadOnlyRepository walletReadRepo, ITransactionNumberGenerator transactionNumberGenerator = null, IUnitOfWork unitOfWork = null, IWalletWriteOnlyRepository walletWriteRepo = null, IMapper mapper = null)
        {
            _loggedUser = loggedUser;
            _transferPasswordValidator = transferPasswordValidator;
            _transactionWriteRepo = transactionWriteRepo;
            _walletReadRepo = walletReadRepo;
            _transactionNumberGenerator = transactionNumberGenerator;
            _unitOfWork = unitOfWork;
            _walletWriteRepo = walletWriteRepo;
        }

        public async Task<ResponseTransfer> Execute(RequestTransfer request)
        {
            await Validate(request);
            var user = await _loggedUser.User();
            
            await _transferPasswordValidator.Validate(user.Id, request.TransactionPassword);
            var sentWallet = await _walletReadRepo.FindWalletByUserId(user.Id);

            if (request.Amount > sentWallet.Balance)
                throw new AmountNotAvaliableExecption();

            var receiverWallet = await _walletReadRepo.FindWalletByCpf(request.ReceiverCpf) ?? throw new NotFoundException(ResourceMessageException.RECEIVER_WALLET_NOT_FOUND);

            if (!receiverWallet.Status.Equals(WalletStatus.Active))
                throw new NotActiveWalletException();

            var transactionalNumber = _transactionNumberGenerator.Generate();
            var transactionalDate = DateTime.UtcNow;

            var receiverTransaction = new Domain.Entities.Transaction
            {
                TransactionDate = transactionalDate,
                TransactionNumber = transactionalNumber,
                Status = TransactionStatus.Completed,
                Amount = request.Amount,
                WalletId = receiverWallet.Id,
                RelatedWalletId  = sentWallet.Id,
                Type = TransactionType.TransferReceived
            };

            await _transactionWriteRepo.Add(receiverTransaction);
            await UpdateWallet(receiverWallet, request.Amount, TransactionType.TransferReceived);


            var sentTransaction = new Domain.Entities.Transaction
            {
                TransactionDate = transactionalDate,
                TransactionNumber = transactionalNumber,
                Status = TransactionStatus.Completed,
                Amount = request.Amount,
                WalletId = sentWallet.Id,
                RelatedWalletId = receiverWallet.Id,
                Type = TransactionType.TransferSent
            };

            await _transactionWriteRepo.Add(sentTransaction);
            await UpdateWallet(sentWallet, request.Amount, TransactionType.TransferSent);

            await _unitOfWork.Commit();

            return new ResponseTransfer
            {
                Amount = request.Amount,
                Date = transactionalDate,
                Description = request.Description,
                SenderName = user.Name,
                ReceiverCpf = request.ReceiverCpf,
                Status = TransactionStatus.Completed.ToString(),
                TransactionNumber = transactionalNumber
            };
        }

        private async Task UpdateWallet(WalletEntity wallet, decimal amount, TransactionType type)
        {
            if (type.Equals(TransactionType.TransferReceived))
                wallet.Balance += amount;

            if (type.Equals(TransactionType.TransferSent))
                wallet.Balance -= amount;

            await _walletWriteRepo.Update(wallet);
        }

        private async Task Validate(RequestTransfer request)
        {
            var validator = new DoTransferValidator();
            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
}
}
