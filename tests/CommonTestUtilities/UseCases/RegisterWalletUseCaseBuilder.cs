using Moq;
using Wallet.Application.UseCases.Wallet.Register;
using Wallet.Domain.Entities;

namespace CommonTestUtilities.UseCases
{
    public class RegisterWalletUseCaseBuilder
    {
        public static IRegisterWalletUseCase Build()
        {
            var mock = new Mock<IRegisterWalletUseCase>();

            mock.Setup(x => x.Execute(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            return mock.Object;
        }
    }
}
