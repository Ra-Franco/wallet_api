using CommonTestUtilities.Requests;
using FluentAssertions;
using Wallet.Application.UseCases.Transaction.Transfer;
using Wallet.Application.UseCases.Transaction.Withdraw;
using Wallet.Communication.Requests.Transactions.Withdraw;
using Wallet.Exceptions;

namespace Validators.Test.Transaction.DoWithdraw
{
    public class DoWithdrawValidatorTest
    {
        [Fact]
        public async Task Success()
        {
            var request = RequestCreateWithdrawBuilder.Build();
            var validator = new DoWithdrawValidator();

            var response = validator.Validate(request);

            response.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task Error_Transactional_Description_Max_Length()
        {
            var request = RequestCreateWithdrawBuilder.Build(characteres: 504);
            var validator = new DoWithdrawValidator();

            var response = validator.Validate(request);

            response.IsValid.Should().BeFalse();
            response.Errors.Should().ContainSingle()
                .And.Contain(e => e.ErrorMessage.Equals(ResourceMessageException.TRANSACTIONAL_DESCRIPTION_MAX_LENGTH));
        }

        [Fact]
        public async Task Error_Amount_Greater_Than_0()
        {
            var request = RequestCreateWithdrawBuilder.Build();
            var validator = new DoWithdrawValidator();
            request.Amount = "-1";

            var response = validator.Validate(request);

            response.IsValid.Should().BeFalse();
            response.Errors.Should().ContainSingle()
                .And.Contain(e => e.ErrorMessage.Equals(ResourceMessageException.TRASANCTIONAL_AMOUNT_GREATER_THAN_0));
        }


        [Fact]
        public async Task Error_Transactional_Password_Invalid()
        {
            var request = RequestCreateWithdrawBuilder.Build();
            var validator = new DoWithdrawValidator();
            request.TransactionPassword = "1111111";

            var response = validator.Validate(request);

            response.IsValid.Should().BeFalse();
            response.Errors.Should().ContainSingle()
                .And.Contain(e => e.ErrorMessage.Equals(ResourceMessageException.TRANSACTIONAL_PASSWORD_LENGTH));
        }
    }
}
