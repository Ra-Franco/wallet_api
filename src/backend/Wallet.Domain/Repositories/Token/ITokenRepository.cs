using Wallet.Domain.Entities;

namespace Wallet.Domain.Repositories.Token
{
    public interface ITokenRepository
    {
        public Task<RefreshToken?> Get(string refreshToken);
        public Task SaveNewRefreshToken(RefreshToken refreshToken);
    }
}
