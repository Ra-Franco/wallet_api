using Moq;
using Wallet.Domain.Security.TransferPassword;
using Wallet.Exceptions.TransactionalPassword;

namespace CommonTestUtilities.Services
{
    public class TransferPasswordValidatorBuilder
    {
        public static ITransferPasswordValidator Build(long userId, string transactionalPassword)
        {
            var mock = new Mock<ITransferPasswordValidator>();
            mock.Setup(x => x.Validate(userId, transactionalPassword))
                .Returns(Task.CompletedTask);

            return mock.Object;
        }

        public static ITransferPasswordValidator BuildWithInvalidPassword()
        {
            var mock = new Mock<ITransferPasswordValidator>();
            mock.Setup(x => x.Validate(It.IsAny<long>(), It.IsAny<string>()))
                .Throws(new InvalidTransactionalPassword());

            return mock.Object;
        }
    }
}
