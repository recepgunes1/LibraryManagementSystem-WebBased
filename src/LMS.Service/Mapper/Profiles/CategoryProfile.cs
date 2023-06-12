using AutoMapper;
using LMS.Entity.Entities;
using LMS.Entity.ViewModels.Category;

namespace LMS.Service.Mapper.Profiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CreateCategoryViewModel>()
            .ReverseMap();
        CreateMap<Category, IndexCategoryViewModel>()
            .ForMember(dest => dest.AmountOfBooks, opt => opt.MapFrom(src => src.Books.Count(p => !p.IsDeleted)))
            .ReverseMap();
        CreateMap<Category, UpdateCategoryViewModel>()
            .ReverseMap();
    }
}