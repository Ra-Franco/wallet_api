namespace Wallet.Domain.Services.Transactional;

public interface IUserSecurityTransactional
{
    public Task ValidateTransactionalSecurity(decimal transactionValue, long userId);
}