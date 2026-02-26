using Microsoft.AspNetCore.Mvc.Filters;

namespace Wallet.Api.Filters
{
    public class AuthenticatorUserFilter : IAsyncAuthorizationFilter
    {

        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            throw new NotImplementedException();
        }
    }
}
