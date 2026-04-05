using CommonTestUtilities.Entities;
using FluentMigrator;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Reflection;
using Testcontainers.MySql;
using Wallet.Domain.Entities;
using Wallet.Domain.Enum;
using Wallet.Domain.Security.Cryptography;
using Wallet.Infrasctructure.Migrations;
using Wallet.Infrasctucture.DataAccess;

namespace WebApi.Test
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly MySqlContainer _mySqlContainer = new MySqlBuilder()
            .WithImage("mysql:8.0.45")
            .WithDatabase("wallet")
            .WithUsername("root")
            .WithPassword("teste123")
            .Build();

        private string _password = string.Empty;
        private User _user = default!;
        private WalletEntity _wallet = default!;
        private IList<Transaction> _transactionList = default!;
        private IPasswordEncrypt _passwordEncrypt = default!;
        private string _transactionalPassowrd = string.Empty;
        private User _userReceiver = default!;
        private WalletEntity _walletReceiver = default!;
        private string _transactionNumber = string.Empty;
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test")
                .ConfigureServices(services =>
                {
                    var dbDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<WalletDbContext>));
                    if (dbDescriptor is not null)
                        services.Remove(dbDescriptor);

                    services.AddDbContext<WalletDbContext>(options =>
                        options.UseMySql(
                            _mySqlContainer.GetConnectionString(),
                            ServerVersion.AutoDetect(_mySqlContainer.GetConnectionString())));

                    using var scope = services.BuildServiceProvider().CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<WalletDbContext>();
                    _passwordEncrypt = scope.ServiceProvider.GetRequiredService<IPasswordEncrypt>();

                    dbContext.Database.EnsureCreated();
                    StartDatabase(dbContext, services);
                });
        }
        public string getCpf() => _user.CPF;
        public string getPassword() => _password;
        public Guid getUserIdentifier() => _user.UserIdentifier;
        public string getTransactionalPassword() => _transactionalPassowrd;
        public string getTransactionNumber() => _transactionNumber;

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

            _transactionNumber = _transactionList.First().TransactionNumber;

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

        public async Task InitializeAsync() => await _mySqlContainer.StartAsync();

        async Task IAsyncLifetime.DisposeAsync() => await _mySqlContainer.DisposeAsync();
    }
}
