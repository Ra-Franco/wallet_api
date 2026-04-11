using Wallet.Communication.Responses.User.Security;

namespace Wallet.Application.UseCases.User.Security.Get
{
    public interface IGetUserSecuritySettings
    {
        public Task<ResponseUserSecuritySettings> Execute();
    }
}
