using Wallet.Communication.Responses.User.Security;
using Wallet.Communication.Utils;
using Wallet.Domain.Repositories;
using Wallet.Domain.Repositories.UserSecuritySettings;
using Wallet.Domain.Services.LoggedUser;

namespace Wallet.Application.UseCases.User.Security.Get
{
    public class GetUserSecuritySettings : IGetUserSecuritySettings
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IUserSecuritySettingRepository _userSecurityRepo;
        private readonly IUnitOfWork _unitOfWork;

        public GetUserSecuritySettings(ILoggedUser loggedUser, IUserSecuritySettingRepository userSecurityRepo, IUnitOfWork unitOfWork)
        {
            _loggedUser = loggedUser;
            _userSecurityRepo = userSecurityRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseUserSecuritySettings> Execute()
        {
            var user = await _loggedUser.User();
            var userSettings = await _userSecurityRepo.GetSettingByUserId(user.Id);
            if (userSettings == null) {  
                userSettings = new Domain.Entities.UserSecuritySettings { 
                    UserId = user.Id, 
                    TransactionLimit = 0, 
                    TransactionLimitPeriod = Domain.Enum.TransactionLimitPeriod.DAYTIME
                };
                await _userSecurityRepo.Add(userSettings);
                await _unitOfWork.Commit();
            }

            return new ResponseUserSecuritySettings
            {
                TransacionLimitPeriod = userSettings.TransactionLimitPeriod.ToString(),
                TransactionLimit = userSettings.TransactionLimit.DecimalToStringCurrency(),
            };
        }
    }
}
