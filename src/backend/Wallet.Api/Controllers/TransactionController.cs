using Microsoft.AspNetCore.Mvc;
using Wallet.Api.Attributes;
using Wallet.Api.Filters;
using Wallet.Application.UseCases.Transaction.Deposits;
using Wallet.Application.UseCases.Transaction.Get;
using Wallet.Application.UseCases.Transaction.GetById;
using Wallet.Application.UseCases.Transaction.Transfer;
using Wallet.Application.UseCases.Transaction.Withdraw;
using Wallet.Communication.Requests.Transactions;
using Wallet.Communication.Requests.Transactions.Deposit;
using Wallet.Communication.Requests.Transactions.Transfer;
using Wallet.Communication.Requests.Transactions.Withdraw;
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
        [ProducesResponseType(typeof(ResponseShortTransaction), StatusCodes.Status201Created)]
        public async Task<IActionResult> Deposit(
            [FromBody] RequestCreateDeposit request,
            [FromServices] ICreateDepositUseCase useCase
            )
        {
            var response = await useCase.Execute(request);
            return Created(string.Empty, response);
        }

        [HttpGet]
        [AuthenticadedUser]
        [ProducesResponseType(typeof(List<ResponseShortTransaction>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTransactions(
            [FromQuery] RequestTransactionsFilter requestFilter,
            [FromQuery] PageParameters pageParameters,
            [FromServices] IGetTransactionsUseCase useCase
            )
        {
            var response = await useCase.Execute(requestFilter, pageParameters);
            return Ok(response);
        }

        [HttpPost("transfer")]
        [AuthenticadedUser]
        [RequireActiveWallet]
        [ProducesResponseType(typeof(ResponseTransfer), StatusCodes.Status201Created)]
        public async Task<IActionResult> DoTransfer(
            [FromServices] IDoTransferUseCase useCase,
            [FromBody] RequestTransfer request
            )
        {
            var response = await useCase.Execute(request);
            return Created(string.Empty, response);
        }

        [HttpPost("withdraw")]
        [AuthenticadedUser]
        [RequireActiveWallet]
        [ProducesResponseType(typeof(ResponseShortTransaction), StatusCodes.Status201Created)]
        public async Task<IActionResult> DoWithdraw(
                [FromServices] IDoWithdrawUseCase useCase,
                [FromBody] RequestCreateWithdraw request
            )
        {
            var response = await useCase.Execute(request);
            return Created(string.Empty, response);
        }

        [HttpGet("{transactionNumber}")]
        [AuthenticadedUser]
        [RequireActiveWallet]
        [ProducesResponseType(typeof(ResponseTransaction), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTransactionByTransactionNumber(
                [FromRoute] string transactionNumber,
                [FromServices] IGetTransactionByTransactionNumberUseCase useCase
            )
        {
            var response = await useCase.Execute(transactionNumber);
            return Ok(response);
        }
    }
}
