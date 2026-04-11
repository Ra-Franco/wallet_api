using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Wallet.Api.Attributes;
using Wallet.Application.UseCases.User.Security.Get;
using Wallet.Application.UseCases.User.Security.Update;
using Wallet.Communication.Requests.User.Security;
using Wallet.Communication.Responses.User.Security;

namespace Wallet.Api.Controllers
{
    [Route("user/security")]
    [AuthenticadedUser]
    public class UserSecuritySettingsController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(ResponseUserSecuritySettings), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSettings(
            [FromServices] IGetUserSecuritySettings useCase
            )
        {
            var response = await useCase.Execute();
            return Ok(response);
        }

        [HttpPatch]
        [ProducesResponseType(typeof(ResponseUserSecuritySettings), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateSettings(
                [FromBody] JsonPatchDocument<RequestUpdateSecuritySettings> request,
                [FromServices] IUpdateUserSettingsUseCase useCase
            )
        {
            await useCase.Execute(request);
            return NoContent();
        }
    }
}
