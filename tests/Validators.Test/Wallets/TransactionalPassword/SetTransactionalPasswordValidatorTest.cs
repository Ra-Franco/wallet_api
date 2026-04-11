using CommonTestUtilities.Requests;
using FluentAssertions;
using Wallet.Application.UseCases.Wallet.TransactionalPassword;
using Wallet.Exceptions;

namespace Validators.Test.Wallets.TransactionalPassword
{
    public class SetTransactionalPasswordValidatorTest
    {
        [Fact]
        public void Success()
        {
            var validator = new SetTransactionalPasswordValidator();
            var request = RequestSetTransactionPasswordJsonBuilder.Build();
            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Error_Transactional_Password_Not_Null()
        {
            var validator = new SetTransactionalPasswordValidator();
            var request = RequestSetTransactionPasswordJsonBuilder.Build();
            request.TransactionPassword = string.Empty;
            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage.Equals(ResourceMessageException.TRANSACTIONAL_PASSWORD_NOTNULL));
        }

        [Fact]
        public void Error_Transactional_Password_Only_Numbers()
        {
            var validator = new SetTransactionalPasswordValidator();
            var request = RequestSetTransactionPasswordJsonBuilder.Build();
            request.TransactionPassword = "TESTE1";
            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(e => e.ErrorMessage.Equals(ResourceMessageException.TRANSACTIONAL_PASSWORD_ONLY_NUMBER));
        }

        [Fact]
        public void Error_Transactional_()
        {
            var validator = new SetTransactionalPasswordValidator();
            var request = RequestSetTransactionPasswordJsonBuilder.Build();
            request.TransactionPassword = "TESTE1";
            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(e => e.ErrorMessage.Equals(ResourceMessageException.TRANSACTIONAL_PASSWORD_ONLY_NUMBER));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void Error_Transaction_Password_Length(int passwordLength)
        {
            var validator = new SetTransactionalPasswordValidator();
            var request = RequestSetTransactionPasswordJsonBuilder.Build(passwordLength);

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(e => e.ErrorMessage.Equals(ResourceMessageException.TRANSACTIONAL_PASSWORD_LENGTH));
        }
    }
}
