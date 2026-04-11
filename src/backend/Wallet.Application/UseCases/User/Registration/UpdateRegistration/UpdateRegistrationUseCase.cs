using Microsoft.AspNetCore.JsonPatch;
using Wallet.Communication.Requests.User;
using Wallet.Communication.Utils;
using Wallet.Domain.Repositories;
using Wallet.Domain.Repositories.User;
using Wallet.Domain.Services.LoggedUser;
using Wallet.Exceptions;
using Wallet.Exceptions.ExceptionsBase;

namespace Wallet.Application.UseCases.User.Registration.UpdateRegistration
{
    public class UpdateRegistrationUseCase : IUpdateRegistrationUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IUserRepositoryWriteOnly _userWriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepositoryReadOnly _userReadRepository;

        public UpdateRegistrationUseCase(ILoggedUser loggedUser, IUserRepositoryWriteOnly userWriteRepository, IUnitOfWork unitOfWork, IUserRepositoryReadOnly userReadRepository)
        {
            _loggedUser = loggedUser;
            _userWriteRepository = userWriteRepository;
            _unitOfWork = unitOfWork;
            _userReadRepository = userReadRepository;
        }

        public async Task Execute(JsonPatchDocument<RequestUpdateRegistrationUser> patchDoc)
        {
            var user = await _loggedUser.User();

            var request = new RequestUpdateRegistrationUser
            {
                Email = user.Email,
                Income = user.Income.DecimalToStringCurrency(),
                Occupation = user.Occupation,
                Phonenumber = user.Phonenumber
            };
            patchDoc.ApplyTo(request);

            await Validator(request);

            user.Email = request.Email;
            user.Income = request.Income.StringToDecimalCurrency();
            user.Occupation = request.Occupation;
            user.Phonenumber = request.Phonenumber;

            await _userWriteRepository.Update(user);
            await _unitOfWork.Commit();
        } 

        public async Task Validator(RequestUpdateRegistrationUser request)
        {
            var validator = new UpdateRegistrationValidator();
            var result = validator.Validate(request);

            var emailExist = await _userReadRepository.ExistUserWithEmail(request.Email);

            if (emailExist)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessageException.EMAIL_ALREADY_REGISTERED));

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
 