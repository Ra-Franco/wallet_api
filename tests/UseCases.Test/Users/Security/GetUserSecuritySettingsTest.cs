using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.UserSecuritySettings;
using CommonTestUtilities.Services;
using FluentAssertions;
using Wallet.Application.UseCases.User.Security.Get;
using Wallet.Communication.Utils;
using Wallet.Domain.Entities;

namespace UseCases.Test.Users.Security;

public class GetUserSecuritySettingsTest
{
    [Fact]
    public async Task Success()
    {
        (var user, var _) = UserBuilder.Build();
        var userSecuritySettings = UserSecuritySettingsBuilder.Build(user);
        var useCase = CreateUseCase(user, userSecuritySettings);
        
        var response = await useCase.Execute();
        response.Should().NotBeNull();
        response.TransacionLimitPeriod.Should().NotBeNull();
        response.TransactionLimit.StringToDecimalCurrency().Should().BeGreaterThan(-1);
    }
    
    [Fact]
    public async Task Success_With_No_Register_Before()
    {
        (var user, var _) = UserBuilder.Build();
        var useCase = CreateUseCase(user);
        
        var response = await useCase.Execute();
        response.Should().NotBeNull();
        response.TransacionLimitPeriod.Should().NotBeNull();
        response.TransactionLimit.StringToDecimalCurrency().Should().BeGreaterThan(-1);
    }
    
    private static GetUserSecuritySettings CreateUseCase(User user, UserSecuritySettings? userSecuritySettings = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var unitOfWork = UnitOfWorkBuilder.Build();
        var repository = new UserSecuritySettingsRepositoryBuilder();
        if (userSecuritySettings != null)
            repository.GetSettingByUserId(user.Id, userSecuritySettings);
        
        return new GetUserSecuritySettings(loggedUser, repository.Build(), unitOfWork);
    }
}