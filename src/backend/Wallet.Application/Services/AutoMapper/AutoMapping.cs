using AutoMapper;
using System.Globalization;
using Wallet.Communication.Requests;
using Wallet.Domain.Entities;

namespace MyRecipeBook.Application.Services.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            RequestToDomain();
        }
        public void RequestToDomain()
        {
            CreateMap<RequestRegisterUserJson, User>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.Income, opt => opt.MapFrom(src => 
                                                            decimal.Parse(src.Income, CultureInfo.InvariantCulture)));
        }
    }
}
