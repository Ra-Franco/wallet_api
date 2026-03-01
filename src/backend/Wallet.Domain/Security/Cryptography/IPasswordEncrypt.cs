namespace Wallet.Domain.Security.Cryptography
{
    public interface IPasswordEncrypt
    {
        string Encrypt(string password);
    }
}
