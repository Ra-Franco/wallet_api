using Wallet.Domain.Enum;
using Wallet.Domain.Repositories.Transactions;
using Wallet.Domain.Repositories.UserSecuritySettings;
using Wallet.Domain.Security.TransferPassword;
using Wallet.Domain.Services.LoggedUser;
using Wallet.Domain.Services.Transactional;
using Wallet.Exceptions.TransactionalException;

namespace Wallet.Application.Services.Transactional;

public class UserSecurityTransactional : IUserSecurityTransactional
{
    private readonly IUserSecuritySettingRepository _userSecuritySettingRepository;
    
    public UserSecurityTransactional(IUserSecuritySettingRepository userSecuritySettingRepository)
    {
        _userSecuritySettingRepository = userSecuritySettingRepository;
    }
    
    public async Task ValidateTransactionalSecurity(decimal transactionValue, long userId)
    {
        var userSettings = await _userSecuritySettingRepository.GetSettingByUserId(userId);
        
        if (userSettings is null || userSettings.TransactionLimit < 1)
            return;

        if (!IsInDayTimePeriod(userSettings.TransactionLimitPeriod))
        {
            if (transactionValue > userSettings.TransactionLimit)
                throw new TransacionLimitExceedException();
        }
    }

    private static bool IsInDayTimePeriod(TransactionLimitPeriod period)
    {
        var dateTime = DateTime.Now;
        switch (period)
        {
            case TransactionLimitPeriod.DAYTIME:
            {
                if (dateTime.Hour is >= 6 and < 20)
                    return true;
                break;   
            }

            case TransactionLimitPeriod.NIGHTH:
            {
                if (dateTime.Hour is >= 6 and < 22)
                    return true;
                break;
            }
        }
        return false;
    }
}