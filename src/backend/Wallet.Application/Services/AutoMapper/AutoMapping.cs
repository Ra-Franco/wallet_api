using AutoMapper;
using System.Globalization;
using Wallet.Communication.Requests;
using Wallet.Communication.Responses.Wallet;
using Wallet.Domain.Entities;

namespace MyRecipeBook.Application.Services.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            RequestToDomain();
            DomainToResponse();
        }
        public void RequestToDomain()
        {
            CreateMap<RequestRegisterUserJson, User>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.Income, opt => opt.MapFrom(src => 
                                                            decimal.Parse(src.Income, CultureInfo.InvariantCulture)));
        }

        private void DomainToResponse()
        {
            CreateMap<Wallet.Domain.Entities.WalletEntity, ResponseWalletDashboard>()
                .ForMember(dest => dest.HasTransactionPassword,
                    opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.TransactionPassword)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}
