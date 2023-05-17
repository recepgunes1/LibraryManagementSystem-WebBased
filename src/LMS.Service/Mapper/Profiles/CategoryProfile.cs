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
            .ForMember(dest => dest.ParentCategory, opt => opt.MapFrom(src => src.ParentCategory!.Name))
            .ReverseMap();
        CreateMap<Category, UpdateCategoryViewModel>()
            .ReverseMap();
    }
}
