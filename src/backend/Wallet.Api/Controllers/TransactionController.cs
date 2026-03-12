using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet.Api.Attributes;
using Wallet.Api.Filters;
using Wallet.Application.UseCases.Transaction.Deposits;
using Wallet.Application.UseCases.Transaction.Get;
using Wallet.Communication.Requests.Deposit;
using Wallet.Communication.Requests.Transactions;
using Wallet.Communication.Responses.Transaction;
using Wallet.Domain.Utils.Page;

namespace Wallet.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        [HttpPost("deposit")]
        [AuthenticadedUser]
        [RequireActiveWallet]
        [ProducesResponseType(typeof(ResponseTransaction), StatusCodes.Status201Created)]
        public async Task<IActionResult> Deposit(
            [FromBody] RequestCreateDeposit request,
            [FromServices] ICreateDepositUseCase useCase
            )
        {
            var response = await useCase.Execute(request);
            return Created(string.Empty,response);
        }

        [HttpGet("deposit")]
        [AuthenticadedUser]
        [ProducesResponseType(typeof(List<ResponseTransaction>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDeposits(
            [FromQuery] RequestTransactionsFilter requestFilter,
            [FromQuery] PageParameters pageParameters,
            [FromServices] IGetDeposits useCase 
            )
        {
            var response = await useCase.Execute(requestFilter, pageParameters);
            return Ok(response);
        }
    }
}
