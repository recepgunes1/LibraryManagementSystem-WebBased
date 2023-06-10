using AutoMapper;
using LMS.Data.UnitOfWorks;
using LMS.Entity.Entities;
using LMS.Entity.ViewModels.Book;
using LMS.Entity.ViewModels.Borrow;
using LMS.Service.Services.Abstracts;

namespace LMS.Service.Services.Concretes;

public class BookService : IBookService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;

    public BookService(IMapper mapper, IUnitOfWork unitOfWork, IUserService userService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userService = userService;
    }

    public async Task<bool> CreateNewBookAsync(CreateBookViewModel viewModel)
    {
        if (await _unitOfWork.GetRepository<Book>().AnyAsync(p => p.Isbn == viewModel.Isbn)) return false;
        var mapped = _mapper.Map<Book>(viewModel);
        mapped.CreatedId = await _userService.GetCurrentUserId();
        await _unitOfWork.GetRepository<Book>().AddAsync(mapped);
        await _unitOfWork.SaveAsync();
        return true;
    }

    public async Task<bool> UpdateBookAsync(UpdateBookViewModel viewModel)
    {
        if (await _unitOfWork.GetRepository<Book>()
                .AnyAsync(p => p.Isbn == viewModel.Isbn && p.Id != viewModel.Id))
            return false;

        var flag = await _unitOfWork.GetRepository<Book>().AnyAsync(p => p.Id == viewModel.Id);

        if (!flag) return false;

        var book = await _unitOfWork.GetRepository<Book>().GetAsync(p => p.Id == viewModel.Id);
        _mapper.Map(viewModel, book);
        book.UpdatedId = await _userService.GetCurrentUserId();
        book.UpdateDateTime = DateTime.Now;
        await _unitOfWork.SaveAsync();
        return true;
    }

    public async Task<bool> DeleteBookWithIdAsync(string id)
    {
        var flag = await _unitOfWork.GetRepository<Book>().AnyAsync(p => p.Id == id);

        if (!flag) return false;

        var book = await _unitOfWork.GetRepository<Book>().GetAsync(p => p.Id == id);
        book.DeletedId = await _userService.GetCurrentUserId();
        book.DeleteDateTime = DateTime.Now;
        book.IsDeleted = true;

        await _unitOfWork.SaveAsync();
        return true;
    }

    public async Task<IEnumerable<IndexBookViewModel>> GetAllBooksNonDeletedAsync()
    {
        var books = await _unitOfWork.GetRepository<Book>()
            .GetAllAsync(p => !p.IsDeleted, i => i.Category, i => i.Author, i => i.Publisher);
        var mapped = _mapper.Map<List<IndexBookViewModel>>(books);
        return mapped;
    }

    public async Task<IEnumerable<IndexBookViewModel>> GetBorrowableBooksAsync()
    {
        var userId = await _userService.GetCurrentUserId();

        var books = await _unitOfWork.GetRepository<Book>()
            .GetAllAsync(p => !p.IsDeleted, i => i.Category, i => i.Author, i => i.Publisher);

        var borrows = await _unitOfWork.GetRepository<Borrow>().GetAllAsync(p => p.UserId == userId &&
            !p.IsDeleted && !(p.IsReturned && p.IsApproved));

        var borrowedBookIds = borrows.Select(b => b.BookId).ToList();

        books = books.Where(book => !borrowedBookIds.Contains(book.Id)).ToList();
        var mapped = _mapper.Map<List<IndexBookViewModel>>(books);
        return mapped;
    }

    public async Task<IEnumerable<UserBorrowViewModel>> GetBorrowedBookHistoryAsync()
    {
        var userId = await _userService.GetCurrentUserId();
        var borrows = await _unitOfWork.GetRepository<Borrow>().GetAllAsync(p => p.UserId == userId &&
            !p.IsDeleted, b => b.Book);
        var mapped = _mapper.Map<List<UserBorrowViewModel>>(borrows);
        return mapped;
    }

    public async Task<UpdateBookViewModel?> GetBookByIdWithUpdateViewModelAsync(string id)
    {
        var book = await _unitOfWork.GetRepository<Book>().GetAsync(p => p.Id == id);
        var mapped = _mapper.Map<UpdateBookViewModel>(book);
        return mapped;
    }

    public async Task<DetailBookViewModel?> GetBookByIdWithDetailBookViewModelAsync(string id)
    {
        var book = await _unitOfWork.GetRepository<Book>()
            .GetAsync(p => p.Id == id, a => a.Author, p => p.Publisher, c => c.Category);
        var mapped = _mapper.Map<DetailBookViewModel>(book);
        return mapped;
    }
}