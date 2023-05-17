using AutoMapper;
using LMS.Entity.Entities;
using LMS.Entity.ViewModels.Publisher;

namespace LMS.Service.Mapper.Profiles;

public class PublisherProfile : Profile
{
    public PublisherProfile()
    {
        CreateMap<Publisher, CreatePublisherViewModel>()
            .ReverseMap();
        CreateMap<Publisher, IndexPublisherViewModel>()
            .ForMember(dest => dest.AmountOfBooks, opt => opt.MapFrom(src => src.Books.Count))
            .ReverseMap();
        CreateMap<Publisher, UpdatePublisherViewModel>()
            .ReverseMap();
    }
}
