using AutoMapper;
using LMS.Entity.Entities;
using LMS.Entity.ViewModels.Author;

namespace LMS.Service.Mapper.Profiles;

public class AuthorProfile : Profile
{
    public AuthorProfile()
    {
        CreateMap<Author, CreateAuthorViewModel>().ReverseMap();
        CreateMap<Author, IndexAuthorViewModel>()
            .ForMember(dest => dest.AmountOfBooks, opt => opt.MapFrom(src => src.Books.Count(p => !p.IsDeleted)))
            .ReverseMap();
        CreateMap<Author, UpdateAuthorViewModel>().ReverseMap();
    }
}