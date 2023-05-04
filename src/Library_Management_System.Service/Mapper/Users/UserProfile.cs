using AutoMapper;
using Library_Management_System.Entity.Entities;
using Library_Management_System.Entity.ViewModels.User;

namespace Library_Management_System.Service.Mapper.Users;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, LoginViewModel>().ReverseMap();
        CreateMap<User, RegisterViewModel>().ReverseMap();
        CreateMap<User, EditUserProfileViewModel>().ReverseMap();
    }
}