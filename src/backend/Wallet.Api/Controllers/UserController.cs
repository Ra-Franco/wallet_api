using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Wallet.Api.Attributes;
using Wallet.Application.UseCases.User.Register;
using Wallet.Application.UseCases.User.UpdateRegistration;
using Wallet.Communication.Requests.User;
using Wallet.Communication.Responses;

namespace Wallet.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseUserRegister), StatusCodes.Status201Created)]
        public async Task<IActionResult> Register(
            [FromServices] IRegisterUserUseCase useCase,
            [FromBody] RequestRegisterUserJson requestBody
            )
        {
            var result = await useCase.Execute(requestBody);

            return Created(string.Empty, result);
        }

        [HttpPatch("registration")]
        [AuthenticadedUser]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateRegistration(
                [FromBody] JsonPatchDocument<RequestUpdateRegistrationUser> request,
                [FromServices] IUpdateRegistrationUseCase useCase
            )
        {
            await useCase.Execute(request);
            return NoContent();
        }
    }
}
