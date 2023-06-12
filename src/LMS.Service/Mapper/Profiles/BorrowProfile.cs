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
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => GetStatusForIndexViewModel(src.IsReturned, src.IsApproved)))
            .ForMember(dest => dest.ButtonVisibility, opt => opt.MapFrom(src => src.IsReturned & !src.IsApproved))
            .ReverseMap();

        CreateMap<Borrow, UserBorrowViewModel>()
            .ForMember(dest => dest.Book, opt => opt.MapFrom(src => src.Book.Name))
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => GetStatusForUserViewModel(src.IsReturned, src.IsApproved)))
            .ForMember(dest => dest.ButtonVisibility, opt => opt.MapFrom(src => !src.IsReturned && !src.IsApproved))
            .ReverseMap();
    }

    private string GetStatusForUserViewModel(bool isReturned, bool isApproved)
    {
        if (isReturned && isApproved)
            return "Returned";

        if (isReturned && !isApproved)
            return "WaitingToApprove";

        if (!isReturned && !isApproved)
            return "InYourLibrary";

        return "Unknown";
    }

    private string GetStatusForIndexViewModel(bool isReturned, bool isApproved)
    {
        if (isReturned && isApproved)
            return "Approved";

        if (isReturned && !isApproved)
            return "WaitingToApprove";

        if (!isReturned && !isApproved)
            return "InTheUser";

        return "Unknown";
    }
}