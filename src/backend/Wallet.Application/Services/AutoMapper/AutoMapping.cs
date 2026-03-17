using AutoMapper;
using System.Globalization;
using Wallet.Communication.Requests;
using Wallet.Communication.Requests.Transactions.Transfer;
using Wallet.Communication.Responses.Transaction;
using Wallet.Communication.Responses.Wallet;
using Wallet.Domain.Entities;
using Wallet.Domain.Utils.Page;

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
            CreateMap<WalletEntity, ResponseWalletDashboard>()
                .ForMember(dest => dest.HasTransactionPassword,
                    opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.TransactionPassword)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<Transaction, ResponseTransaction>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.TransactionDate));
            CreateMap<PagedList<Transaction>, PagedList<ResponseTransaction>>()
                .ConvertUsing((src, _, context) =>
                    new PagedList<ResponseTransaction>(
                        context.Mapper.Map<List<ResponseTransaction>>(src.Items),
                        src.Page,
                        src.PageSize,
                        src.TotalCount
                    ));
        }
    }
}
