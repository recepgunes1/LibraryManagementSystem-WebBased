using AutoMapper;
using LMS.Entity.Entities;
using LMS.Entity.ViewModels.User;

namespace LMS.Service.Mapper.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, LoginViewModel>().ReverseMap();
        CreateMap<User, RegisterViewModel>().ReverseMap();
        CreateMap<User, EditUserProfileViewModel>().ReverseMap();
        CreateMap<User, CreateUserViewModel>().ReverseMap();
        CreateMap<User, UpdateUserViewModel>().ReverseMap();
        CreateMap<User, IndexUserViewModel>()
            .ForMember(dest => dest.AmountOfBooks,
                opt => opt.MapFrom(src => src.Borrows.Count(p => p.Id == src.Id && !p.IsReturned)))
            .ReverseMap();
    }
}
