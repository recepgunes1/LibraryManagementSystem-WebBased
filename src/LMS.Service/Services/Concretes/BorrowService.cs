using AutoMapper;
using LMS.Data.UnitOfWorks;
using LMS.Entity.Entities;
using LMS.Entity.ViewModels.Borrow;
using LMS.Service.Services.Abstracts;

namespace LMS.Service.Services.Concretes;

public class BorrowService : IBorrowService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public BorrowService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }


    public async Task<IEnumerable<IndexBorrowViewModel>> GetAllBorrowsAsync()
    {
        var publishers = await _unitOfWork.GetRepository<Borrow>()
            .GetAllAsync(p => !p.IsDeleted, i => i.Book, i => i.User);
        var mapped = _mapper.Map<List<IndexBorrowViewModel>>(publishers);
        return mapped;
    }
}