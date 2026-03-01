using Microsoft.AspNetCore.Mvc;
using Wallet.Application.UseCases.Auth.Login;
using Wallet.Communication.Requests.Login;
using Wallet.Communication.Responses;
using Wallet.Communication.Responses.Token;

namespace Wallet.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseTokenJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(
            [FromBody] RequestLoginJson requestBody,
            [FromServices] IDoLoginUseCase useCase
            )
        {
            var response = await useCase.Execute(requestBody);
            return Ok(response);
        }
    }
}
