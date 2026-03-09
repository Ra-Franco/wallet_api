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

        public static IRuleBuilderOptions<T, string>TransactionPasswordIsValid<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotNull().WithMessage(ResourceMessageException.TRANSACTIONAL_PASSWORD_NOTNULL)
                .NotEmpty().WithMessage(ResourceMessageException.TRANSACTIONAL_PASSWORD_NOTNULL)
                .Length(6).WithMessage(ResourceMessageException.TRANSACTIONAL_PASSWORD_LENGTH)
                .Matches(@"^\d+$").WithMessage(ResourceMessageException.TRANSACTIONAL_PASSWORD_ONLY_NUMBER);
        }

        public static IRuleBuilderOptions<T, decimal>TransactionalAmountValidator<T>(this IRuleBuilder<T, decimal> ruleBuilder)
        {
            return ruleBuilder
                .NotNull().WithMessage(ResourceMessageException.TRANSACTIONAL_AMOUNT_NOT_NULL)
                .NotEmpty().WithMessage(ResourceMessageException.TRANSACTIONAL_AMOUNT_NOT_NULL)
                .GreaterThan(0).WithMessage(ResourceMessageException.TRASANCTIONAL_AMOUNT_GREATER_THAN_0);
        }
    }
}
