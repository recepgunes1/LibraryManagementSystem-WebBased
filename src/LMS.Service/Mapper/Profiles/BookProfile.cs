using AutoMapper;
using LMS.Entity.Entities;
using LMS.Entity.ViewModels.Book;

namespace LMS.Service.Mapper.Profiles;

public class BookProfile : Profile
{
    public BookProfile()
    {
        CreateMap<Book, CreateBookViewModel>().ReverseMap();
        CreateMap<Book, UpdateBookViewModel>().ReverseMap();
    }
}