using Microsoft.AspNetCore.JsonPatch;
using Wallet.Communication.Requests.User;

namespace Wallet.Application.UseCases.User.Registration.UpdateRegistration
{
    public interface IUpdateRegistrationUseCase
    {
        public Task Execute(JsonPatchDocument<RequestUpdateRegistrationUser> request);
    }
}
