using AutoMapper;
using LMS.Entity.Entities;
using LMS.Entity.ViewModels.Book;

namespace LMS.Service.Mapper.Profiles;

public class BookProfile : Profile
{
    public BookProfile()
    {
        CreateMap<Book, CreateBookViewModel>().ReverseMap();
        CreateMap<Book, UpdateBookViewModel>()
            .ForMember(dest => dest.ImagePath,
                opt => opt.MapFrom(src => Path.Combine(Path.DirectorySeparatorChar.ToString(), "images",
                    src.Image.FolderName, src.Image.FileName)))
            .ReverseMap();
        CreateMap<Book, IndexBookViewModel>()
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.FullName))
            .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.Publisher.Name))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.ImagePath,
                opt => opt.MapFrom(src => Path.Combine(Path.DirectorySeparatorChar.ToString(), "images",
                    src.Image.FolderName, src.Image.FileName)))
            .ReverseMap();
        CreateMap<Book, DetailBookViewModel>()
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.FullName))
            .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.Publisher.Name))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.ImagePath,
                opt => opt.MapFrom(src => Path.Combine(Path.DirectorySeparatorChar.ToString(), "images",
                    src.Image.FolderName, src.Image.FileName)))
            .ReverseMap();
    }
}