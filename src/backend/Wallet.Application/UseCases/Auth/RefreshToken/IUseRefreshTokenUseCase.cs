using Wallet.Communication.Requests.Login.Token;
using Wallet.Communication.Responses.Token;

namespace Wallet.Application.UseCases.Auth.RefreshToken
{
    public interface IUseRefreshTokenUseCase
    {
        public Task<ResponseTokenJson> Execute(RequestNewTokenJson request);
    }
}
