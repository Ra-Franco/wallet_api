using FluentValidation;
using Wallet.Communication.Utils;
using Wallet.Exceptions;

namespace Wallet.Application.UseCases
{
    public static class FluentValidationExtensions
    {
        public static IRuleBuilderOptions<T, string>CpfIsValid<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must(cpf => CpfUtils.IsValid(cpf))
                .WithMessage(ResourceMessageException.CPF_INVALID);
        }
    }
}
