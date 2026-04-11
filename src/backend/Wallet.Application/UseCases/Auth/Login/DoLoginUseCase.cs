using Wallet.Communication.Requests.Login;
using Wallet.Communication.Responses.Token;
using Wallet.Domain.Repositories;
using Wallet.Domain.Repositories.Token;
using Wallet.Domain.Repositories.User;
using Wallet.Domain.Security.Cryptography;
using Wallet.Domain.Security.Tokens;
using Wallet.Exceptions.Login;

namespace Wallet.Application.UseCases.Auth.Login
{
    public class DoLoginUseCase : IDoLoginUseCase
    {
        private readonly IUserRepositoryReadOnly _repository;
        private readonly IPasswordEncrypt _passwordEncrypt;
        private readonly IAccessTokenGenerator _tokenAccess;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly ITokenRepository _tokenRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DoLoginUseCase(IUserRepositoryReadOnly repository, IPasswordEncrypt passwordEncrypt, IAccessTokenGenerator tokenAccess, IRefreshTokenGenerator refreshTokenGenerator, ITokenRepository tokenRepository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _passwordEncrypt = passwordEncrypt;
            _tokenAccess = tokenAccess;
            _refreshTokenGenerator = refreshTokenGenerator;
            _tokenRepository = tokenRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseTokenJson> Execute(RequestLoginJson request)
        {
            var encryptedUser = _passwordEncrypt.Encrypt(request.Password);
            var user = await _repository.ExistActiveUserWithCpfAndPassword(request.Cpf, encryptedUser) ?? throw new InvalidLoginException();

            var refreshToken = await CreateAndSaveRefreshToken(user);

            return new ResponseTokenJson
            {
                AccessToken = _tokenAccess.Generate(user.UserIdentifier),
                RefreshToken = refreshToken
            };
        }

        private async Task<string> CreateAndSaveRefreshToken(Domain.Entities.User user)
        {
            var refreshToken = new Domain.Entities.RefreshToken
            {
                Value = _refreshTokenGenerator.Generate(),
                UserId = user.Id,
            };

            await _tokenRepository.SaveNewRefreshToken(refreshToken);

            await _unitOfWork.Commit();

            return refreshToken.Value;
        }
    }
}
