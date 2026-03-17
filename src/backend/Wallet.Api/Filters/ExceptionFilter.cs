using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using Wallet.Communication.Responses;
using Wallet.Exceptions;
using Wallet.Exceptions.Wallet;

namespace Wallet.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is WalletException walletException)
            {
                HandleProjectException(context, walletException);
            } else
            {
                ThrowUnkowException(context);
            }
        }

        private static void ThrowUnkowException(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessageException.UNKNOW_ERROR));
        }

        private static void HandleProjectException(ExceptionContext context, WalletException walletException)
        {
            context.HttpContext.Response.StatusCode = (int)walletException.GetStatusCode();
            context.Result = new ObjectResult(new ResponseErrorJson(walletException.GetErrorMessages()));
        }
    }
}
