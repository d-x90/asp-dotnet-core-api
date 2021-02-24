using AutoMapper;
using AspDotnetCoreApi.Dtos;
using AspDotnetCoreApi.Models;

namespace AspDotnetCoreApi.Profiles {
    public class UserProfile : Profile {
        public UserProfile()
        {
            CreateMap<AuthRegisterRequestDto, User>()
                .ForMember(x => x.Email, opt => opt.MapFrom(source => source.Email.ToLower().Trim()))
                .ForMember(x => x.Username, opt => opt.MapFrom(source => source.Username.ToLower().Trim()))
                .ForMember(x => x.FirstName, opt => opt.MapFrom(source => source.FirstName.Trim()))
                .ForMember(x => x.LastName, opt => opt.MapFrom(source => source.LastName.Trim()));
        }
    }
}