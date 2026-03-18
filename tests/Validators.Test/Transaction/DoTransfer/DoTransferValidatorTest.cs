using CommonTestUtilities.Requests;
using FluentAssertions;
using Wallet.Application.UseCases.Transaction.Transfer;
using Wallet.Exceptions;

namespace UseCases.Test.Transactions.DoTransfer
{
    public class DoTransferValidatorTest
    {
        [Fact]
        public async Task Success()
        {
            var request = RequestTransferBuilder.Build();
            var validator = new DoTransferValidator();

            var response = validator.Validate(request);

            response.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task Error_Transactional_Description_Max_Length()
        {
            var request = RequestTransferBuilder.Build(characteres: 504);
            var validator = new DoTransferValidator();

            var response = validator.Validate(request);

            response.IsValid.Should().BeFalse();
            response.Errors.Should().ContainSingle()
                .And.Contain(e => e.ErrorMessage.Equals(ResourceMessageException.TRANSACTIONAL_DESCRIPTION_MAX_LENGTH));
        }

        [Fact]
        public async Task Error_Amount_Greater_Than_0()
        {
            var request = RequestTransferBuilder.Build();
            var validator = new DoTransferValidator();
            request.Amount = -1;

            var response = validator.Validate(request);

            response.IsValid.Should().BeFalse();
            response.Errors.Should().ContainSingle()
                .And.Contain(e => e.ErrorMessage.Equals(ResourceMessageException.TRASANCTIONAL_AMOUNT_GREATER_THAN_0));
        }

        [Fact]
        public async Task Error_Cpf_Invalid()
        {
            var request = RequestTransferBuilder.Build();
            var validator = new DoTransferValidator();
            request.ReceiverCpf = "11111111122";

            var response = validator.Validate(request);

            response.IsValid.Should().BeFalse();
            response.Errors.Should().ContainSingle()
                .And.Contain(e => e.ErrorMessage.Equals(ResourceMessageException.CPF_INVALID));
        }

        [Fact]
        public async Task Error_Transactional_Password_Invalid()
        {
            var request = RequestTransferBuilder.Build();
            var validator = new DoTransferValidator();
            request.TransactionPassword = "1111111";

            var response = validator.Validate(request);

            response.IsValid.Should().BeFalse();
            response.Errors.Should().ContainSingle()
                .And.Contain(e => e.ErrorMessage.Equals(ResourceMessageException.TRANSACTIONAL_PASSWORD_LENGTH));
        }
    }
}
