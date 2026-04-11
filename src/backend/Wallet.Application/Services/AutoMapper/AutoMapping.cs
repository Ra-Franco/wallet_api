using AutoMapper;
using Wallet.Communication.Responses.Transaction;
using Wallet.Communication.Responses.Wallet;
using Wallet.Domain.Entities;
using Wallet.Domain.Utils.Page;
using Wallet.Communication.Utils;
using Wallet.Communication.Requests.User;
using Wallet.Communication.Responses.User;

namespace Wallet.Application.Services.AutoMapper
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
                .ForMember(dest => dest.Income, opt => opt.MapFrom(src => src.Income.StringToDecimalCurrency()));
        }

        private void DomainToResponse()
        {
            CreateMap<WalletEntity, ResponseWalletDashboard>()
                .ForMember(dest => dest.HasTransactionPassword,
                    opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.TransactionPassword)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<Transaction, ResponseShortTransaction>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.TransactionDate))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount.DecimalPrecision()));

            CreateMap<PagedList<Transaction>, PagedList<ResponseShortTransaction>>()
                .ConvertUsing((src, _, context) =>
                    new PagedList<ResponseShortTransaction>(
                        context.Mapper.Map<List<ResponseShortTransaction>>(src.Items),
                        src.Page,
                        src.PageSize,
                        src.TotalCount
                    ));

            CreateMap<Transaction, ResponseTransaction>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));

            CreateMap<User, ResponseUserRegistration>()
                .ForMember(dest => dest.Income, opt => opt.MapFrom(src => src.Income.DecimalToStringCurrency()));
        }
    }
}
