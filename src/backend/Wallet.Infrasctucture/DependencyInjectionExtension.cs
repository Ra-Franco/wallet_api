using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Wallet.Domain.Repositories;
using Wallet.Domain.Repositories.User;
using Wallet.Infrasctructure.DataAccess;
using Wallet.Infrasctructure.DataAccess.Repositories.User;
using Wallet.Infrasctructure.Extensions;
using Wallet.Infrasctucture.DataAccess;


namespace Wallet.Infrasctucture
{
    public static class DependencyInjectionExtension
    {
        public static void AddInfrasctructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddRepositories(services);
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
        }

        private static void AddFluentMigration(IServiceCollection services, string connectionString)
        {
            services.AddFluentMigratorCore().ConfigureRunner(options =>
            {
                options.AddMySql5()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("Wallet.Infrasctructure")).For.All();
            });
        }
    }
}
