namespace Wallet.Application.UseCases.Wallet.Register
{
    public interface IRegisterWalletUseCase
    {
        public Task Execute(Domain.Entities.User user);
    }
}
