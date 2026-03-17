using Wallet.Domain.Repositories.Wallet;
using Wallet.Domain.Security.Cryptography;
using Wallet.Domain.Security.TransferPassword;
using Wallet.Exceptions.TransactionalPassword;

namespace Wallet.Application.Services.PasswordTransactional
{
    public class TransferPasswordValidator : ITransferPasswordValidator
    {
        private readonly IWalletReadOnlyRepository _walletReadRepository;
        private readonly IPasswordEncrypt _passwordEncrypt;

        public TransferPasswordValidator(IWalletReadOnlyRepository walletReadRepository, IPasswordEncrypt passwordEncrypt)
        {
            _walletReadRepository = walletReadRepository;
            _passwordEncrypt = passwordEncrypt;
        }

        public async Task Validate(long userId, string transactionalPassword)
        {
            var userTransactionalPassword = await _walletReadRepository.GetTransactionalPasswordByUserId(userId) ?? throw new TransactionPasswordNotFound();
            var passwordEncrypt = _passwordEncrypt.Encrypt(transactionalPassword);
            if (passwordEncrypt != userTransactionalPassword)
                throw new InvalidTransactionalPassword();
        }
    }
}
