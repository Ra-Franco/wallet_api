namespace Wallet.Domain.Repositories.User
{
    public interface IUserRepositoryReadOnly
    {
        public Task<bool> ExistUserWithEmail(string email);
        public Task<bool> ExistUserWithCpf(string cpf);

        public Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier);
        public Task<Entities.User?> ExistActiveUserWithCpfAndPassword(string cpf, string password);
    }
}
