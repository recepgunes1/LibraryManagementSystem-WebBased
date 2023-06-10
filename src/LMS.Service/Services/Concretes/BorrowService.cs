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
    private readonly IUserService _userService;

    public BorrowService(IMapper mapper, IUnitOfWork unitOfWork, IUserService userService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userService = userService;
    }

    public async Task<IEnumerable<IndexBorrowViewModel>> GetAllBorrowsAsync()
    {
        var borrows = await _unitOfWork.GetRepository<Borrow>()
            .GetAllAsync(p => !p.IsDeleted, i => i.Book, i => i.User);
        var mapped = _mapper.Map<List<IndexBorrowViewModel>>(borrows);
        return mapped;
    }

    public async Task BorrowAsync(string bookId)
    {
        var userId = await _userService.GetCurrentUserId();
        var book = await _unitOfWork.GetRepository<Book>().GetByIdAsync(bookId);
        await _unitOfWork.GetRepository<Borrow>().AddAsync(new Borrow
        {
            UserId = userId,
            BookId = bookId,
            BorrowDateTime = DateTime.Now,
            ReturnDateTime = DateTime.Now.AddDays(await _userService.GetMaxDaysClaimAsync()),
            CreateDateTime = DateTime.Now,
            CreatedId = userId
        });
        book.Amount -= 1;
        await _unitOfWork.SaveAsync();
    }

    public async Task ReturnAsync(string id)
    {
        var borrow = await _unitOfWork.GetRepository<Borrow>().GetByIdAsync(id);
        borrow.IsReturned = true;
        await _unitOfWork.SaveAsync();
    }

    public async Task ApproveAsync(string id)
    {
        var borrow = await _unitOfWork.GetRepository<Borrow>().GetByIdAsync(id);
        borrow.IsApproved = true;
        borrow.ReturnDateTime = DateTime.Now;
        borrow.UpdatedId = await _userService.GetCurrentUserId();
        borrow.UpdateDateTime = DateTime.Now;
        var book = await _unitOfWork.GetRepository<Book>().GetByIdAsync(borrow.BookId);
        book.Amount += 1;
        await _unitOfWork.SaveAsync();
    }
}