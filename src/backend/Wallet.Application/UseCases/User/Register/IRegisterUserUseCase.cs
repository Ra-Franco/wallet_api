using Wallet.Communication.Requests.User;
using Wallet.Communication.Responses.User;

namespace Wallet.Application.UseCases.User.Register
{
    public interface IRegisterUserUseCase
    {
        public Task<ResponseUserRegister> Execute(RequestRegisterUserJson request);
    }
}
