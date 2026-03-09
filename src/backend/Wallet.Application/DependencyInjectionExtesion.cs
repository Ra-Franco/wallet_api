using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Application.Services.AutoMapper;
using MyRecipeBook.Application.Services.Cryptography;
using Wallet.Application.UseCases.Auth.Login;
using Wallet.Application.UseCases.Transaction.Deposits;
using Wallet.Application.UseCases.User.Register;
using Wallet.Application.UseCases.Wallet.Add;
using Wallet.Application.UseCases.Wallet.CreateTransactionalPassword;
using Wallet.Application.UseCases.Wallet.Get;
using Wallet.Application.UseCases.Wallet.Register;
using Wallet.Application.UseCases.Wallet.TransactionalPassword;

namespace Wallet.Application
{
    public static class DependencyInjectionExtesion
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            AddAutomapper(services);
            AddUseCases(services);
            AddPasswordEncrypter(services, configuration);
        }

        private static void AddAutomapper(IServiceCollection services)
        {
            services.AddScoped(option => new AutoMapper.MapperConfiguration(option =>
            {
                option.AddProfile(new AutoMapping());
            }).CreateMapper());
        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
            services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
            services.AddScoped<IRegisterWalletUseCase, RegisterWalletUseCase>();
            services.AddScoped<ISetTransactionalPassword, SetTransactionalPassword>();
            services.AddScoped<IWalletDasboardUseCase, WalletDasboardUseCase>();
            services.AddScoped<ICreateDepositUseCase, CreateDepositUseCase>();
        }

        private static void AddPasswordEncrypter(IServiceCollection services, IConfiguration configuration)
        {
            var salt = configuration.GetValue<string>("Settings:Password:Salt");
            services.AddScoped(option => new PasswordEncrypter(salt!));
        }
    }   
}
