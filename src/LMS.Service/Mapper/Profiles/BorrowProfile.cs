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
            .ForMember(dest => dest.IsVisible, opt => opt.MapFrom(src => src.IsReturned && !src.IsApproved))
            .ReverseMap();

        CreateMap<Borrow, UserBorrowViewModel>()
            .ForMember(dest => dest.Book, opt => opt.MapFrom(src => src.Book.Name))
            .ForMember(dest => dest.IsVisible, opt => opt.MapFrom(src => !(src.IsApproved && src.IsReturned)))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => GetStatus(src.IsReturned, src.IsApproved)))
            .ReverseMap();
    }

    private string GetStatus(bool isReturned, bool isApproved)
    {
        if (isReturned && isApproved)
            return "Returned";

        if (isReturned && !isApproved)
            return "WaitingToApprove";

        if (!isReturned && !isApproved)
            return "InYourLibrary";

        // Default status if none of the conditions match
        return "Unknown";
    }
}