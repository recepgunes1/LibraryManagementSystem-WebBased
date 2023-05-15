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
    }
}