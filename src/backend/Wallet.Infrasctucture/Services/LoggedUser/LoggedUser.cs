using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Wallet.Application.Services.LoggedUser;
using Wallet.Application.Tokens;
using Wallet.Domain.Entities;
using Wallet.Domain.Enum;
using Wallet.Infrasctucture.DataAccess;

namespace Wallet.Infrasctructure.Services.LoggedUser
{
    public class LoggedUser : ILoggedUser
    {

        private readonly WalletDbContext _db;
        private readonly ITokenProvider _tokenProvider;

        public LoggedUser(WalletDbContext db, ITokenProvider tokenProvider)
        {
            _db = db;
            _tokenProvider = tokenProvider;
        }

        public async Task<User> User()
        {
            var token = _tokenProvider.Value();
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

            string identifier = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

            var userIdentifer = Guid.Parse(identifier);

            return await _db
                .Users
                .AsNoTracking()
                .FirstAsync(user => user.Status == UserStatus.Active && user.UserIdentifier == userIdentifer);
        }
    }
}
