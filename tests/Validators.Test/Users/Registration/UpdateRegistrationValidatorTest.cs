using CommonTestUtilities.Requests;
using FluentAssertions;
using Wallet.Application.UseCases.User.Registration.UpdateRegistration;
using Wallet.Exceptions;

namespace Validators.Test.Users.Registration
{
    public class UpdateRegistrationValidatorTest
    {
        [Fact]
        public void Success()
        {
            var validator = new UpdateRegistrationValidator();
            var request = RequestUpdateRegistrationUserBuilder.Build();
            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Error_Email_Empty()
        {
            var validator = new UpdateRegistrationValidator();
            var request = RequestUpdateRegistrationUserBuilder.Build();
            request.Email = string.Empty;
            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(e => e.ErrorMessage.Equals(ResourceMessageException.EMAIL_EMPTY));
        }

        [Fact]
        public void Error_Email_Invalid()
        {
            var validator = new UpdateRegistrationValidator();
            var request = RequestUpdateRegistrationUserBuilder.Build();
            request.Email = "string.Empty";
            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(e => e.ErrorMessage.Equals(ResourceMessageException.EMAIL_INVALID));
        }

        [Fact]
        public void Error_Occupation_Empty()
        {
            var validator = new UpdateRegistrationValidator();
            var request = RequestUpdateRegistrationUserBuilder.Build();
            request.Occupation = string.Empty;
            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(e => e.ErrorMessage.Equals(ResourceMessageException.OCCUPATION_EMPTY));
        }

        [Fact]
        public void Error_Income_Empty()
        {
            var validator = new UpdateRegistrationValidator();
            var request = RequestUpdateRegistrationUserBuilder.Build();
            request.Income = string.Empty;
            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(e => e.ErrorMessage.Equals(ResourceMessageException.INCOME_EMPTY));
        }

        [Fact]
        public void Error_Phone_Number_Invalid()
        {
            var validator = new UpdateRegistrationValidator();
            var request = RequestUpdateRegistrationUserBuilder.Build();
            request.Phonenumber = "(99)21115-5555";
            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(e => e.ErrorMessage.Equals(ResourceMessageException.PHONE_NUMBER_INVALID));
        }
    }
}
