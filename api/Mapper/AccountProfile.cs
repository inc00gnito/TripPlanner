using api.Models;
using api.Models.DTOs;
using AutoMapper;

namespace api.Mapper
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<RegisterModel, Account>()
                .ForMember(
                    dest => dest.Password,
                    opt => opt.MapFrom(src => src.Password));
            CreateMap<Account, AccountDto>();
        }
    }
}
