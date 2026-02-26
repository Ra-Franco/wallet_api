using Microsoft.AspNetCore.Mvc;
using Wallet.Application.UseCases.User.Register;
using Wallet.Communication.Requests;
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
            [FromServices]IRegisterUserUseCase useCase,
            [FromBody]RequestRegisterUserJson requestBody
            )
        {
           var result = await useCase.Execute(requestBody);

           return Created(string.Empty, result);
        }
    }
}
