using Wallet.Communication.Requests;
using Wallet.Communication.Responses;

namespace Wallet.Application.UseCases.User.Register
{
    public interface IRegisterUserUseCase
    {
        public Task<ResponseUserRegister> Execute(RequestRegisterUserJson request);
    }
}
