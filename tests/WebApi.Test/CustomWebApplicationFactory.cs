using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Wallet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Wallet.Infrasctucture.DataAccess;
using System.Linq;

namespace WebApi.Test
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private string _password = string.Empty;
        private User _user = default!;
        private WalletEntity _wallet = default!;
        private IList<Transaction> _transactionList = default!;
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

                    dbContext.Database.EnsureDeleted();

                    StartDatabase(dbContext);
                });
        }
        public string getCpf() => _user.CPF;
        public string getPassword() => _password;
        public Guid getUserIdentifier() => _user.UserIdentifier;
        private void StartDatabase(WalletDbContext dbContext)
        {
            (_user, _password) = UserBuilder.Build();
            _wallet = WalletBuilder.Build(_user);
            _transactionList = TransactionBuilder.BuildList(_wallet.Id);
            dbContext.Users.Add(_user);
            dbContext.Wallet.Add(_wallet);
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
