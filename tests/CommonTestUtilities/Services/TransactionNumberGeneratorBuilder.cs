using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Moq;
using System.Reflection.Metadata.Ecma335;
using Wallet.Domain.Services.TransactionNumber;

namespace CommonTestUtilities.Services
{
    public class TransactionNumberGeneratorBuilder
    {
       public static ITransactionNumberGenerator Build()
        {
            var mock = new Mock<ITransactionNumberGenerator>();
            mock.Setup(m => m.Generate()).Returns($"TRX{Guid.NewGuid().ToString()}");
            return mock.Object;
        }
    }
}
