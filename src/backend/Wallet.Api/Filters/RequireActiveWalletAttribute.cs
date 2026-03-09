using Microsoft.AspNetCore.Mvc.Filters;
using Wallet.Application.Services.LoggedUser;
using Wallet.Domain.Enum;
using Wallet.Domain.Repositories.Wallet;
using Wallet.Exceptions;
using Wallet.Exceptions.ExceptionsBase;

namespace Wallet.Api.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireActiveWalletAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var loggedUser = context.HttpContext.RequestServices.GetRequiredService<ILoggedUser>();
            var walletRepository = context.HttpContext.RequestServices.GetRequiredService<IWalletReadOnlyRepository>();

            var user = await loggedUser.User();

            var wallet = await walletRepository.FindWalletByUserId(user.Id);

            if (wallet is null)
                throw new NotFoundException(ResourceMessageException.WALLET_NOT_FOUND);

            if (wallet.Status != WalletStatus.Active)
                throw new NotActiveWalletException(); 

            await next();
        }
    }
}
