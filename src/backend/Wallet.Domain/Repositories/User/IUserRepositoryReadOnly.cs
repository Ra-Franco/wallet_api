namespace Wallet.Domain.Repositories.User
{
    public interface IUserRepositoryReadOnly
    {
        public Task<bool> ExistActiveUserWithEmail(string email);
        public Task<bool> ExistActiveUserWithCpf(string cpf); 
    }
}
