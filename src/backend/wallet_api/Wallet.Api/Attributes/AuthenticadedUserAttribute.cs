using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Wallet.Api.Filters;

namespace Wallet.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthenticadedUserAttribute : TypeFilterAttribute
    {
        public AuthenticadedUserAttribute() : base(typeof(AuthenticatorUserFilter))
        {
        }
    }
}
