using Wallet.Communication.Responses.User;

namespace Wallet.Application.UseCases.User.Registration.Get
{
    public interface IGetUserRegistration
    {
        public Task<ResponseUserRegistration> Execute();
    }
}
