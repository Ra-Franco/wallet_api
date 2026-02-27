using AutoMapper;
using MyRecipeBook.Application.Services.Cryptography;
using System.ComponentModel.DataAnnotations;
using Wallet.Application.Tokens;
using Wallet.Communication.Requests;
using Wallet.Communication.Responses;
using Wallet.Communication.Responses.Token;
using Wallet.Domain.Entities;
using Wallet.Domain.Repositories;
using Wallet.Domain.Repositories.User;
using Wallet.Exceptions;
using Wallet.Exceptions.ExceptionsBase;

namespace Wallet.Application.UseCases.User.Register
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IUserRepositoryReadOnly _readRepository;
        private readonly IUserRepositoryWriteOnly _writeRepository;
        private readonly IMapper _mapper;
        private readonly PasswordEncrypter _passwordEncrypter;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccessTokenGenerator _tokenAccess;

        public RegisterUserUseCase(
            IUserRepositoryReadOnly readRepository, 
            IUserRepositoryWriteOnly writeRepository, 
            IMapper mapper, 
            PasswordEncrypter passwordEncrypter, 
            IUnitOfWork unitOfWork,
            IAccessTokenGenerator accessTokenGenerator)
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
            _mapper = mapper;
            _passwordEncrypter = passwordEncrypter;
            _unitOfWork = unitOfWork;
            _tokenAccess = accessTokenGenerator;
        }

        public async Task<ResponseUserRegister> Execute(RequestRegisterUserJson request)
        {
            await Validate(request);
            var user = _mapper.Map<Domain.Entities.User>(request);
            user.Password = _passwordEncrypter.Encrypt(request.Password);
            user.UserIdentifier = Guid.NewGuid();

            await _writeRepository.Add(user);
            await _unitOfWork.Commit();
            return new ResponseUserRegister
            {
                Name = user.Name,
                Tokens = new ResponseTokenJson
                {
                    AccessToken = _tokenAccess.Generate(user.UserIdentifier)
                }
            };
        }

        private async Task Validate(RequestRegisterUserJson request)
        {
            var validator = new RegisterUserValidator();
            var result = validator.Validate(request);

            var emailExist = await _readRepository.ExistActiveUserWithEmail(request.Email);

            if (emailExist)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessageException.EMAIL_ALREADY_REGISTERED));
            var cpfExist = await _readRepository.ExistActiveUserWithCpf(request.CPF);
            if (cpfExist)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessageException.CPF_ALREADY_EXIST));

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
