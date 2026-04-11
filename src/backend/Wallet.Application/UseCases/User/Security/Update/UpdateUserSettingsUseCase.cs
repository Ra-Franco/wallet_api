using Microsoft.AspNetCore.JsonPatch;
using Wallet.Communication.Requests.User.Security;
using Wallet.Communication.Utils;
using Wallet.Domain.Repositories;
using Wallet.Domain.Repositories.UserSecuritySettings;
using Wallet.Domain.Services.LoggedUser;
using Wallet.Exceptions.ExceptionsBase;

namespace Wallet.Application.UseCases.User.Security.Update
{
    public class UpdateUserSettingsUseCase : IUpdateUserSettingsUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IUserSecuritySettingRepository _userSettingsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserSettingsUseCase(ILoggedUser loggedUser, IUserSecuritySettingRepository userSettingsRepository, IUnitOfWork unitOfWork)
        {
            _loggedUser = loggedUser;
            _userSettingsRepository = userSettingsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(JsonPatchDocument<RequestUpdateSecuritySettings> patchDoc)
        {

            var loggedUser = await _loggedUser.User();
            var settings = await _userSettingsRepository.GetSettingByUserId(loggedUser.Id);
            var request = new RequestUpdateSecuritySettings
            {
                TransactionLimit = settings!.TransactionLimit.DecimalToStringCurrency(),
                TransactionLimitPeriod= settings.TransactionLimitPeriod,
            }; 

            patchDoc.ApplyTo(request);

            Validate(request);

            settings.TransactionLimit = request.TransactionLimit.StringToDecimalCurrency();
            settings.TransactionLimitPeriod = request.TransactionLimitPeriod;

            await _userSettingsRepository.Update(settings);
            await _unitOfWork.Commit();
        }

        private void Validate(RequestUpdateSecuritySettings request)
        {
            var validator = new UpdateUserSettingsValidator();
            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
