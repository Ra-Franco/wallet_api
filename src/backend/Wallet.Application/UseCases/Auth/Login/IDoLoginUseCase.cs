using Wallet.Communication.Requests.Login;
using Wallet.Communication.Responses.Token;

namespace Wallet.Application.UseCases.Auth.Login
{
    public interface IDoLoginUseCase
    {
        public Task<ResponseTokenJson> Execute(RequestLoginJson request);
    }
}
