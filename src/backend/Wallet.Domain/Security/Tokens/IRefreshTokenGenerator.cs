namespace Wallet.Domain.Security.Tokens
{
    public interface IRefreshTokenGenerator
    {
        public string Generate();
    }
}
