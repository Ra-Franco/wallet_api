using Wallet.Application.Services.LoggedUser;
using Wallet.Application.UseCases.Wallet.TransactionalPassword;
using Wallet.Communication.Requests.Wallet;
using Wallet.Domain.Repositories;
using Wallet.Domain.Repositories.Wallet;
using Wallet.Domain.Security.Cryptography;
using Wallet.Exceptions;
using Wallet.Exceptions.ExceptionsBase;

namespace Wallet.Application.UseCases.Wallet.SetTransactionalPassword
{
    public class SetTransactionalPassword : ISetTransactionalPassword
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IWalletWriteOnlyRepository _walletWriteOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWalletReadOnlyRepository _walletReadOnlyRepository;
        private readonly IPasswordEncrypt _passwordEncrypt;

        public SetTransactionalPassword(ILoggedUser loggedUser, IWalletWriteOnlyRepository walletWriteOnlyRepository, IUnitOfWork unitOfWork, IWalletReadOnlyRepository walletReadOnlyRepository, IPasswordEncrypt passwordEncrypt)
        {
            _loggedUser = loggedUser;
            _walletWriteOnlyRepository = walletWriteOnlyRepository;
            _unitOfWork = unitOfWork;
            _walletReadOnlyRepository = walletReadOnlyRepository;
            _passwordEncrypt = passwordEncrypt;
        }

        public async Task Execute(RequestSetTransactionPasswordJson request)
        {
            await Validate(request);
            var user = await _loggedUser.User() ?? throw new UnathorizedException();
            var wallet = await _walletReadOnlyRepository.FindWalletByUserId(user.Id) ?? throw new NotFoundException(ResourceMessageException.WALLET_NOT_FOUND);

            var encryptedPassword = _passwordEncrypt.Encrypt(request.TransactionPassword);
         
            await _walletWriteOnlyRepository.UpdateTransactionPassword(encryptedPassword, wallet.Id);
            await _unitOfWork.Commit();
        }

        private async Task Validate(RequestSetTransactionPasswordJson request)
        {
            var validator = new SetTransactionalPasswordValidator();
            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
