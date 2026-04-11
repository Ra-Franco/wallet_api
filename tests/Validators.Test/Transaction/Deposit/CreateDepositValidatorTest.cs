using CommonTestUtilities.Requests;
using FluentAssertions;
using Wallet.Application.UseCases.Transaction.Deposits;
using Wallet.Exceptions;

namespace Validators.Test.Transaction.Deposit
{
    public class CreateDepositValidatorTest
    {
        [Fact]
        public void Success()
        {
            var request = RequestCreateDepositBuilder.Build();
            var validator = new CreateDepositValidator();

            var response = validator.Validate(request);

            response.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Error_Transactional_Description_Max_Length()
        {
            var request = RequestCreateDepositBuilder.Build(504);
            var validator = new CreateDepositValidator();

            var response = validator.Validate(request);

            response.IsValid.Should().BeFalse();
            response.Errors.Should().ContainSingle()
                .And.Contain(e => e.ErrorMessage.Equals(ResourceMessageException.TRANSACTIONAL_DESCRIPTION_MAX_LENGTH));
        }

        [Fact]
        public void Error_Amount_Greater_Than_0()
        {
            var request = RequestCreateDepositBuilder.Build();
            var validator = new CreateDepositValidator();
            request.Amount = "-1";

            var response = validator.Validate(request);

            response.IsValid.Should().BeFalse();
            response.Errors.Should().ContainSingle()
                .And.Contain(e => e.ErrorMessage.Equals(ResourceMessageException.TRASANCTIONAL_AMOUNT_GREATER_THAN_0));
        }
    }
}
