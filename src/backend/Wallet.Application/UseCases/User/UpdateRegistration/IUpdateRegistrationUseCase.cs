using Microsoft.AspNetCore.JsonPatch;
using Wallet.Communication.Requests.User;

namespace Wallet.Application.UseCases.User.UpdateRegistration
{
    public interface IUpdateRegistrationUseCase
    {
        public Task Execute(JsonPatchDocument<RequestUpdateRegistrationUser> request);
    }
}
