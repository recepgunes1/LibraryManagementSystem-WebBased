using AutoMapper;
using LMS.Entity.Entities;
using LMS.Entity.ViewModels.Borrow;

namespace LMS.Service.Mapper.Profiles;

public class BorrowProfile : Profile
{
    public BorrowProfile()
    {
        CreateMap<Borrow, IndexBorrowViewModel>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
            .ForMember(dest => dest.Book, opt => opt.MapFrom(src => src.Book.Name))
            .ReverseMap();
    }
}