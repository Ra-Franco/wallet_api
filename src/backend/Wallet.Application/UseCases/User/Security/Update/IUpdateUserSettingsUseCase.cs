using Microsoft.AspNetCore.JsonPatch;
using Wallet.Communication.Requests.User.Security;

namespace Wallet.Application.UseCases.User.Security.Update
{
    public interface IUpdateUserSettingsUseCase
    {
        public Task Execute(JsonPatchDocument<RequestUpdateSecuritySettings> request);
    }
}
