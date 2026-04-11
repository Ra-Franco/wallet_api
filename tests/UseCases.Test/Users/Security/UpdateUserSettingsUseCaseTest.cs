using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.UserSecuritySettings;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services;
using FluentAssertions;
using Wallet.Application.UseCases.User.Security.Update;
using Wallet.Domain.Entities;
using Wallet.Domain.Enum;
using Wallet.Exceptions;
using Wallet.Exceptions.ExceptionsBase;

namespace UseCases.Test.Users.Security;

public class UpdateUserSettingsUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, var _) = UserBuilder.Build();
        var userSecuritySettings = UserSecuritySettingsBuilder.Build(user);
        var useCase = CreateUseCase(user, userSecuritySettings);
        
        var request = RequestUpdateSecuritySettingsBuilder.JsonPatchBuild();
        
        Func<Task> act = async () => await useCase.Execute(request);
            
        await act.Should().NotThrowAsync();
    }
    
    [Fact]
    public async Task Validator_Error_Handling()
    {
        (var user, var _) = UserBuilder.Build();
        var userSecuritySettings = UserSecuritySettingsBuilder.Build(user);
        var useCase = CreateUseCase(user, userSecuritySettings);
        
        var request = RequestUpdateSecuritySettingsBuilder.JsonPatchBuild();
        
        request.Operations.First(o => o.path == "/TransactionLimitPeriod").value = (TransactionLimitPeriod)999;

        Func<Task> act = async () => await useCase.Execute(request);
        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.GetErrorMessages().Count == 1 &&
                        e.GetErrorMessages().Contains(ResourceMessageException.TRANSACTION_LIMIT_PERIOD_INVALID));
    }
    
    private static UpdateUserSettingsUseCase CreateUseCase(User user, UserSecuritySettings? userSecuritySettings)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var unitOfWork = UnitOfWorkBuilder.Build();
        var repository = new UserSecuritySettingsRepositoryBuilder();
        if (userSecuritySettings != null)
            repository.GetSettingByUserId(user.Id, userSecuritySettings);
        
        return new UpdateUserSettingsUseCase(loggedUser, repository.Build(), unitOfWork);
    }
}