using Moq;
using Wallet.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories.Users
{
    public class UserReadOnlyRepositoryBuilder
    {
        private readonly Mock<IUserRepositoryReadOnly> _repository;

        public UserReadOnlyRepositoryBuilder() => _repository = new Mock<IUserRepositoryReadOnly>();

        public IUserRepositoryReadOnly Build() => _repository.Object;

        public void ExistActiveUserWithEmail(string email)
        {
            _repository.Setup(repository => repository.ExistUserWithEmail(email)).ReturnsAsync(true);
        }

        public void ExistActiveUserWithCpf(string cpf)
        {
            _repository.Setup(repo => repo.ExistUserWithCpf(cpf)).ReturnsAsync(true);
        }

        public void ExistActiveUserWithCpfAndPassword(Wallet.Domain.Entities.User user)
        {
            _repository.Setup(repo => repo.ExistActiveUserWithCpfAndPassword(user.CPF, user.Password)).ReturnsAsync(user);
        }
    }
}
