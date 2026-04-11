using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wallet.Domain.Repositories;
using Wallet.Domain.Repositories.Token;
using Wallet.Domain.Repositories.Transactions;
using Wallet.Domain.Repositories.User;
using Wallet.Domain.Repositories.UserSecuritySettings;
using Wallet.Domain.Repositories.Wallet;
using Wallet.Domain.Security.Cryptography;
using Wallet.Domain.Security.Tokens;
using Wallet.Domain.Services.LoggedUser;
using Wallet.Domain.Services.TransactionNumber;
using Wallet.Infrasctructure.DataAccess;
using Wallet.Infrasctructure.DataAccess.Repositories.Token;
using Wallet.Infrasctructure.DataAccess.Repositories.Transactions;
using Wallet.Infrasctructure.DataAccess.Repositories.User;
using Wallet.Infrasctructure.DataAccess.Repositories.UserSecuritySettings;
using Wallet.Infrasctructure.DataAccess.Repositories.Wallet;
using Wallet.Infrasctructure.Extensions;
using Wallet.Infrasctructure.Security.Cryptography;
using Wallet.Infrasctructure.Security.Token.Access;
using Wallet.Infrasctructure.Security.Token.Refresh;
using Wallet.Infrasctructure.Services.LoggedUser;
using Wallet.Infrasctructure.Services.TransactionNumber;

namespace Wallet.Infrasctructure
{
    public static class DependencyInjectionExtension
    {
        public static void AddInfrasctructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddTokens(services, configuration);
            AddLoggedUser(services);
            AddRepositories(services);
            AddPasswordEncrypter(services, configuration);
            AddTransactionNumberGenerator(services);
            if (configuration.IsUnitTestEnviroment())
                return;

            var connectionString = configuration.ConnectionString();
            AddDbContext(services, connectionString!);
            AddFluentMigration(services, connectionString!);
        }
        private static void AddDbContext(IServiceCollection services, string connectionString)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 45));

            services.AddDbContext<WalletDbContext>(dbContextOp =>
            {
                dbContextOp.UseMySql(connectionString, serverVersion);
            });
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepositoryReadOnly, UserRepository>();
            services.AddScoped<IUserRepositoryWriteOnly, UserRepository>();
            services.AddScoped<IWalletWriteOnlyRepository, WalletRepository>();
            services.AddScoped<IWalletReadOnlyRepository, WalletRepository>();
            services.AddScoped<ITransactionWriteOnlyRepository, TransactionRepository>();
            services.AddScoped<ITransactionReadOnlyRepository, TransactionRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IUserSecuritySettingRepository, UserSecuritySettingRepository>();
        }

        private static void AddTokens(IServiceCollection services, IConfiguration configuration)
        {
            var expirationTime = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
            var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

            services.AddScoped<IAccessTokenGenerator>(opt => new JwtTokenGenerator(expirationTime, signingKey!));
            services.AddScoped<IAccessTokenValidator>(opt => new JwtTokenValidator(signingKey!));
            services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
        }
        private static void AddLoggedUser(IServiceCollection services) => services.AddScoped<ILoggedUser, LoggedUser>();
        private static void AddFluentMigration(IServiceCollection services, string connectionString)
        {
            services.AddFluentMigratorCore().ConfigureRunner(options =>
            {
                options.AddMySql5()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("Wallet.Infrasctructure")).For.All();
            });
        }

        private static void AddPasswordEncrypter(IServiceCollection services, IConfiguration configuration)
        {
            var salt = configuration.GetValue<string>("Settings:Password:Salt");
            services.AddScoped<IPasswordEncrypt>(option => new Sha512Encrypter(salt!));
        }

        private static void AddTransactionNumberGenerator(IServiceCollection services) => services.AddScoped<ITransactionNumberGenerator, TransactionNumberGenerator>();
    }
}
