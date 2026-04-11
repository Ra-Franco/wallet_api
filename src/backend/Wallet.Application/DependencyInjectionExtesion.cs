using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Wallet.Application.Services.Cryptography;
using Wallet.Application.Services.AutoMapper;
using Wallet.Application.Services.PasswordTransactional;
using Wallet.Application.UseCases.Auth.Login;
using Wallet.Application.UseCases.Transaction.Deposits;
using Wallet.Application.UseCases.Transaction.Get;
using Wallet.Application.UseCases.Transaction.Transfer;
using Wallet.Application.UseCases.User.Register;
using Wallet.Application.UseCases.Wallet.Register;
using Wallet.Application.UseCases.Wallet.TransactionalPassword;
using Wallet.Domain.Security.TransferPassword;
using Wallet.Application.UseCases.Transaction.Withdraw;
using Wallet.Application.UseCases.Transaction.GetById;
using Wallet.Application.UseCases.Auth.RefreshToken;
using Wallet.Application.UseCases.Wallet.GetBalance;
using Wallet.Application.UseCases.User.Registration.UpdateRegistration;
using Wallet.Application.UseCases.User.Registration.Get;
using Wallet.Application.UseCases.User.Security.Get;
using Wallet.Application.UseCases.User.Security.Update;
using Wallet.Application.UseCases.Wallet.Dashboard;

namespace Wallet.Application
{
    public static class DependencyInjectionExtesion
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            AddAutomapper(services);
            AddUseCases(services);
            AddPasswordEncrypter(services, configuration);
            AddTransferPasswordValidator(services);
        }

        private static void AddAutomapper(IServiceCollection services)
        {
            services.AddScoped(option => new AutoMapper.MapperConfiguration(option =>
            {
                option.AddProfile(new AutoMapping());
            }, NullLoggerFactory.Instance).CreateMapper());
        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
            services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
            services.AddScoped<IRegisterWalletUseCase, RegisterWalletUseCase>();
            services.AddScoped<ISetTransactionalPassword, SetTransactionalPassword>();
            services.AddScoped<IWalletDasboardUseCase, WalletDasboardUseCase>();
            services.AddScoped<ICreateDepositUseCase, CreateDepositUseCase>();
            services.AddScoped<IGetTransactionsUseCase, GetTransactionsUseCase>();
            services.AddScoped<IDoTransferUseCase, DoTransferUseCase>();
            services.AddScoped<IDoWithdrawUseCase, DoWithdrawUseCase>();
            services.AddScoped<IGetTransactionByTransactionNumberUseCase, GetTransactionByTransactionNumberUseCase>();
            services.AddScoped<IUseRefreshTokenUseCase, UseRefreshTokenUseCase>();
            services.AddScoped<IGetBalanceUseCase, GetBalanceUseCase>();
            services.AddScoped<IUpdateRegistrationUseCase, UpdateRegistrationUseCase>();
            services.AddScoped<IGetUserRegistration, GetUserRegistration>();
            services.AddScoped<IGetUserSecuritySettings, GetUserSecuritySettings>();
            services.AddScoped<IUpdateUserSettingsUseCase, UpdateUserSettingsUseCase>();
        }

        private static void AddPasswordEncrypter(IServiceCollection services, IConfiguration configuration)
        {
            var salt = configuration.GetValue<string>("Settings:Password:Salt");
            services.AddScoped(option => new PasswordEncrypter(salt!));
        }

        private static void AddTransferPasswordValidator(IServiceCollection services) => services.AddScoped<ITransferPasswordValidator, TransferPasswordValidator>();
    }   
}
