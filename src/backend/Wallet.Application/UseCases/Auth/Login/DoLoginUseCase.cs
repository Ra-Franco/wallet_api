using Wallet.Application.Tokens;
using Wallet.Communication.Requests.Login;
using Wallet.Communication.Responses.Token;
using Wallet.Domain.Repositories.User;
using Wallet.Domain.Security.Cryptography;
using Wallet.Exceptions.Login;

namespace Wallet.Application.UseCases.Auth.Login
{
    public class DoLoginUseCase : IDoLoginUseCase
    {
        private readonly IUserRepositoryReadOnly _repository;
        private readonly IPasswordEncrypt _passwordEncrypt;
        private readonly IAccessTokenGenerator _tokenAccess;

        public DoLoginUseCase(IUserRepositoryReadOnly repository, IPasswordEncrypt passwordEncrypt, IAccessTokenGenerator tokenAccess)
        {
            _repository = repository;
            _passwordEncrypt = passwordEncrypt;
            _tokenAccess = tokenAccess;
        }

        public async Task<ResponseTokenJson> Execute(RequestLoginJson request)
        {
            var encryptedUser = _passwordEncrypt.Encrypt(request.Password);
            var user = await _repository.ExistActiveUserWithCpfAndPassword(request.Cpf, encryptedUser) ?? throw new InvalidLoginException();

            return new ResponseTokenJson
            {
                AccessToken = _tokenAccess.Generate(user.UserIdentifier)
            };
        }
    }
}
