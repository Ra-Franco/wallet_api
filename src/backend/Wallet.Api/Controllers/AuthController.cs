using Microsoft.AspNetCore.Mvc;
using Wallet.Application.UseCases.Auth.Login;
using Wallet.Application.UseCases.Auth.RefreshToken;
using Wallet.Communication.Requests.Login;
using Wallet.Communication.Requests.Login.Token;
using Wallet.Communication.Responses;
using Wallet.Communication.Responses.Token;

namespace Wallet.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
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

        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(ResponseTokenJson), StatusCodes.Status200OK)]
        public async Task<IActionResult> RefreshToken(
                [FromServices] IUseRefreshTokenUseCase useCase,
                [FromBody] RequestNewTokenJson request
            )
        {
            var response = await useCase.Execute(request);
            return Ok(response);
        }
    }
}
