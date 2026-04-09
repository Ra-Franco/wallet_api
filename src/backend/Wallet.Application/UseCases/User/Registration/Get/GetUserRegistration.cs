using AutoMapper;
using Wallet.Application.Services.LoggedUser;
using Wallet.Communication.Responses.User;

namespace Wallet.Application.UseCases.User.Registration.Get
{
    public class GetUserRegistration : IGetUserRegistration
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IMapper _mapper;

        public GetUserRegistration(ILoggedUser loggedUser, IMapper mapper)
        {
            _loggedUser = loggedUser;
            _mapper = mapper;
        }

        public async Task<ResponseUserRegistration> Execute()
        {
            var loggedUser = await _loggedUser.User();

            return _mapper.Map<ResponseUserRegistration>(loggedUser);

        }
    }
}
