using AutoMapper;
using MyRecipeBook.Application.Services.Cryptography;
using System.ComponentModel.DataAnnotations;
using Wallet.Application.Tokens;
using Wallet.Application.UseCases.Wallet.Register;
using Wallet.Communication.Requests.User;
using Wallet.Communication.Responses;
using Wallet.Communication.Responses.Token;
using Wallet.Domain.Entities;
using Wallet.Domain.Enum;
using Wallet.Domain.Repositories;
using Wallet.Domain.Repositories.Token;
using Wallet.Domain.Repositories.User;
using Wallet.Domain.Security.Cryptography;
using Wallet.Domain.Security.Tokens;
using Wallet.Exceptions;
using Wallet.Exceptions.ExceptionsBase;
using Wallet.Communication.Utils;

namespace Wallet.Application.UseCases.User.Register
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IUserRepositoryReadOnly _readRepository;
        private readonly IUserRepositoryWriteOnly _writeRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordEncrypt _passwordEncrypter;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccessTokenGenerator _tokenAccess;
        private readonly IRegisterWalletUseCase _registerWalletUseCase;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly ITokenRepository _tokenRepository;

        public RegisterUserUseCase(
            IUserRepositoryReadOnly readRepository,
            IUserRepositoryWriteOnly writeRepository,
            IMapper mapper,
            IPasswordEncrypt passwordEncrypter,
            IUnitOfWork unitOfWork,
            IAccessTokenGenerator accessTokenGenerator,
            IRegisterWalletUseCase registerWalletUseCase,
            IRefreshTokenGenerator refreshTokenGenerator,
            ITokenRepository tokenRepository)
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
            _mapper = mapper;
            _passwordEncrypter = passwordEncrypter;
            _unitOfWork = unitOfWork;
            _tokenAccess = accessTokenGenerator;
            _registerWalletUseCase = registerWalletUseCase;
            _refreshTokenGenerator = refreshTokenGenerator;
            _tokenRepository = tokenRepository;
        }

        public async Task<ResponseUserRegister> Execute(RequestRegisterUserJson request)
        {
            await Validate(request);
            
            var user = _mapper.Map<Domain.Entities.User>(request);
            user.Password = _passwordEncrypter.Encrypt(request.Password);
            user.UserIdentifier = Guid.NewGuid();
            user.Status = UserStatus.Active;
            
            await _writeRepository.Add(user);

            await _unitOfWork.Commit();

            await _registerWalletUseCase.Execute(user);

            string refreshToken = await CreateAndSaveRefreshToken(user);

            return new ResponseUserRegister
            {
                Name = user.Name,
                Tokens = new ResponseTokenJson
                {
                    AccessToken = _tokenAccess.Generate(user.UserIdentifier),
                    RefreshToken = refreshToken,
                }
            };
        }

        private async Task Validate(RequestRegisterUserJson request)
        {
            var validator = new RegisterUserValidator();
            var result = validator.Validate(request);

            var emailExist = await _readRepository.ExistUserWithEmail(request.Email);

            if (emailExist)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessageException.EMAIL_ALREADY_REGISTERED));
            var cpfExist = await _readRepository.ExistUserWithCpf(request.CPF);
            if (cpfExist)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessageException.CPF_ALREADY_EXIST));

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }

        private async Task<string> CreateAndSaveRefreshToken(Domain.Entities.User user)
        {
            var refreshToken = new RefreshToken
            {
                Value = _refreshTokenGenerator.Generate(),
                UserId = user.Id,
            };

            await _tokenRepository.SaveNewRefreshToken(refreshToken);

            await _unitOfWork.Commit();

            return refreshToken.Value;
        }
    }
}
