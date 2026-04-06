using Moq;
using Wallet.Domain.Entities;
using Wallet.Domain.Repositories.Token;

namespace CommonTestUtilities.Repositories.Token
{
    public class TokenRepositoryBuilder
    {
        private readonly Mock<ITokenRepository> _repository;

        public TokenRepositoryBuilder() => _repository = new Mock<ITokenRepository>();

        public ITokenRepository Build() => _repository.Object;

        public void Get(RefreshToken refreshEntity)
        {
            _repository.Setup(repo => repo.Get(It.IsAny<string>())).ReturnsAsync(refreshEntity);
        }
    }
}
