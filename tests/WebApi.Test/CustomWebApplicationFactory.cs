using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Wallet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Wallet.Infrasctucture.DataAccess;
using Wallet.Domain.Security.Cryptography;
using Wallet.Domain.Enum;

namespace WebApi.Test
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private string _password = string.Empty;
        private User _user = default!;
        private WalletEntity _wallet = default!;
        private IList<Transaction> _transactionList = default!;
        private IPasswordEncrypt _passwordEncrypt = default!;
        private string _transactionalPassowrd = string.Empty;
        private User _userReceiver = default!;
        private WalletEntity _walletReceiver = default!;
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test")
                .ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<WalletDbContext>));
                    if (descriptor is not null)
                        services.Remove(descriptor);

                    var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                    services.AddDbContext<WalletDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                        options.UseInternalServiceProvider(provider);
                    });
                    using var scope = services.BuildServiceProvider().CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<WalletDbContext>();
                    _passwordEncrypt = scope.ServiceProvider.GetRequiredService<IPasswordEncrypt>();

                    dbContext.Database.EnsureDeleted();

                    StartDatabase(dbContext, services);
                });
        }
        public string getCpf() => _user.CPF;
        public string getPassword() => _password;
        public Guid getUserIdentifier() => _user.UserIdentifier;
        public string getTransactionalPassword() => _transactionalPassowrd;

        public string getCpfReceiver() => _userReceiver.CPF;
        private void StartDatabase(WalletDbContext dbContext, IServiceCollection services)
        {
            (_user, _password) = UserBuilder.Build();
            _wallet = WalletBuilder.Build(_user, WalletStatus.Active);
            _transactionList = TransactionBuilder.BuildList(_wallet.Id);

            (_userReceiver, _) = UserBuilder.Build();
            _walletReceiver = WalletBuilder.Build(_userReceiver, WalletStatus.Active);

            dbContext.Users.AddRange(_user,_userReceiver);

            _transactionalPassowrd = _wallet.TransactionPassword;
            _wallet.TransactionPassword = _passwordEncrypt.Encrypt(_wallet.TransactionPassword);
            dbContext.Wallet.AddRange(_wallet, _walletReceiver);
            
            for (var i = 0; i < _transactionList.Count; i++)
                dbContext.Transactions.Add(_transactionList[i]);
            dbContext.SaveChanges();
        }

        public void SetWalletStatus(Guid userIdentifier, Wallet.Domain.Enum.WalletStatus status)
        {
            using var scope = Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<WalletDbContext>();
            var user = db.Users.Single(u => u.UserIdentifier == userIdentifier);
            var wallet = db.Wallet.Single(w => w.UserId == user.Id);
            wallet.Status = status;
            db.SaveChanges();
        }
    }
}
