using CommonTestUtilities.Requests;
using FluentAssertions;
using Wallet.Application.UseCases.User.Security.Update;
using Wallet.Domain.Enum;
using Wallet.Exceptions;

namespace Validators.Test.Users.Security
{
    public class UpdateUserSettingsValidatorTest
    {
        [Fact]
        public void Success()
        {
            var validator = new UpdateUserSettingsValidator();
            var request = RequestUpdateSecuritySettingsBuilder.Build();
            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Error_Transaction_Limit_Period_Invalid()
        {
            var validator = new UpdateUserSettingsValidator();
            var request = RequestUpdateSecuritySettingsBuilder.Build();
            request.TransactionLimitPeriod = (TransactionLimitPeriod) 999;
            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(e => e.ErrorMessage.Equals(ResourceMessageException.TRANSACTION_LIMIT_PERIOD_INVALID));
        }

        [Fact]
        public void Error_Transaction_Limit_Negative()
        {
            var validator = new UpdateUserSettingsValidator();
            var request = RequestUpdateSecuritySettingsBuilder.Build();
            request.TransactionLimit = "-1";
            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(e => e.ErrorMessage.Equals(ResourceMessageException.TRANSACTION_LIMIT_NOT_NEGATIVE));
        }
    }
}
