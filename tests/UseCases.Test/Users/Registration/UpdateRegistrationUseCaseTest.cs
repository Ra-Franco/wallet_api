using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.User;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services;
using FluentAssertions;
using Wallet.Application.UseCases.User.Registration.UpdateRegistration;
using Wallet.Domain.Entities;
using Wallet.Exceptions;
using Wallet.Exceptions.ExceptionsBase;

namespace UseCases.Test.Users.Registration
{
    public class UpdateRegistrationUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, var _) = UserBuilder.Build();
            var request = RequestUpdateRegistrationUserBuilder.JsonPatchBuild();

            var useCase = CreateUseCase(user, "teste@email.com");

            Func<Task> act = async () => await useCase.Execute(request);
            
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Email_Already_Registered()
        {
            (var user, var _) = UserBuilder.Build();
            var request = RequestUpdateRegistrationUserBuilder.JsonPatchBuild();

            var emailFromRequest = request.Operations
                .First(o => o.path == "/Email").value.ToString();

            var useCase = CreateUseCase(user, emailFromRequest);

            Func<Task> act = async () => await useCase.Execute(request);
            (await act.Should().ThrowAsync<ErrorOnValidationException>())
                .Where(e => e.GetErrorMessages().Count == 1 &&
                        e.GetErrorMessages().Contains(ResourceMessageException.EMAIL_ALREADY_REGISTERED));
        }

        private static UpdateRegistrationUseCase CreateUseCase(User user, string? email = null)
        {
            var loggerUser = LoggedUserBuilder.Build(user);
            var writeRepository = UserWriteOnlyRepositoryBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();

            var readRepository = new UserReadOnlyRepositoryBuilder();
            if (!string.IsNullOrEmpty(email))
                readRepository.ExistActiveUserWithEmail(email);

            return new UpdateRegistrationUseCase(loggerUser, writeRepository, unitOfWork, readRepository.Build());        
        }
    }
}
