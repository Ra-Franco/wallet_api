using Wallet.Communication.Requests.Login.Token;
using Wallet.Communication.Responses.Token;
using Wallet.Domain.Repositories;
using Wallet.Domain.Repositories.Token;
using Wallet.Domain.Security.Tokens;
using Wallet.Exceptions.Token;

namespace Wallet.Application.UseCases.Auth.RefreshToken
{
    public class UseRefreshTokenUseCase : IUseRefreshTokenUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccessTokenGenerator _acessTokenGenerator;
        private readonly ITokenRepository _tokenRepository;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;

        public UseRefreshTokenUseCase(IUnitOfWork unitOfWork, IAccessTokenGenerator acessTokenGenerator, ITokenRepository tokenRepository, IRefreshTokenGenerator refreshTokenGenerator)
        {
            _unitOfWork = unitOfWork;
            _acessTokenGenerator = acessTokenGenerator;
            _tokenRepository = tokenRepository;
            _refreshTokenGenerator = refreshTokenGenerator;
        }

        public async Task<ResponseTokenJson> Execute(RequestNewTokenJson request)
        {
            var refreshToken = await _tokenRepository.Get(request.RefreshToken);

            if (refreshToken is null)
                throw new RefreshTokenNotFoundException();

            var refreshTokenValidUntil = refreshToken.Created_On.AddDays(30);
            if (DateTime.Compare(refreshTokenValidUntil, DateTime.UtcNow) < 0)
                throw new RefreshTokenExpiredException();

            var newRefreshToken = new Domain.Entities.RefreshToken
            {
                Value = _refreshTokenGenerator.Generate(),
                UserId = refreshToken.UserId
            };

            await _tokenRepository.SaveNewRefreshToken(newRefreshToken);
            await _unitOfWork.Commit();

            return new ResponseTokenJson
            {
                AccessToken = _acessTokenGenerator.Generate(refreshToken.User.UserIdentifier),
                RefreshToken = newRefreshToken.Value
            };
        }
    }
}
