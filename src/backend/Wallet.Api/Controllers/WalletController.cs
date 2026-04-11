using Microsoft.AspNetCore.Mvc;
using Wallet.Api.Attributes;
using Wallet.Application.UseCases.Wallet.Dashboard;
using Wallet.Application.UseCases.Wallet.GetBalance;
using Wallet.Application.UseCases.Wallet.TransactionalPassword;
using Wallet.Communication.Requests.Wallet;
using Wallet.Communication.Responses.Wallet;

namespace Wallet.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        [HttpPut("set-transactional-password")]
        [AuthenticadedUser]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SetTransactionPassword(
            [FromServices] ISetTransactionalPassword useCase,
            [FromBody] RequestSetTransactionPasswordJson request
            ) 
        {
            await useCase.Execute(request);
            return NoContent();
        }

        [HttpGet]
        [AuthenticadedUser]
        [ProducesResponseType(typeof(ResponseWalletDashboard), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetWalletDashboard(
            [FromServices] IWalletDasboardUseCase useCase
            )
        {
            var response = await useCase.Execute();
            return Ok(response);
        }

        [HttpGet("balance")]
        [AuthenticadedUser]
        [ProducesResponseType(typeof(ResponseGetBalance), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBalance(
                [FromServices] IGetBalanceUseCase useCase
            )
        {
            var response = await useCase.Execute();
            return Ok(response);
        }
    }
}
