using AutoMapper;
using LMS.Data.UnitOfWorks;
using LMS.Entity.Entities;
using LMS.Entity.ViewModels.Book;
using LMS.Entity.ViewModels.Borrow;
using LMS.Service.Helpers.ImageService;
using LMS.Service.Services.Abstracts;

namespace LMS.Service.Services.Concretes;

public class BookService : IBookService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;
    private readonly IImageService _imageService;

    public BookService(IMapper mapper, IUnitOfWork unitOfWork, IUserService userService, IImageService imageService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userService = userService;
        _imageService = imageService;
    }

    public async Task<bool> CreateNewBookAsync(CreateBookViewModel viewModel)
    {
        if (await _unitOfWork.GetRepository<Book>().AnyAsync(p => p.Isbn == viewModel.Isbn)) return false;
        var image = await _imageService.UploadAsync(viewModel.FormFile, "books");
        image.CreatedId = await _userService.GetCurrentUserId();
        var mapped = _mapper.Map<Book>(viewModel);
        mapped.CreatedId = await _userService.GetCurrentUserId();
        mapped.Image = image;
        mapped.ImageId = image.Id;
        await _unitOfWork.GetRepository<Image>().AddAsync(image);
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

        var book = await _unitOfWork.GetRepository<Book>().GetAsync(p => p.Id == viewModel.Id, i => i.Image);
        _mapper.Map(viewModel, book);
        book.UpdatedId = await _userService.GetCurrentUserId();
        book.UpdateDateTime = DateTime.Now;
        if (viewModel.FormFile != null)
        {
            var oldImageId = book.ImageId;
            var image = await _imageService.UploadAsync(viewModel.FormFile, "books");
            image.CreatedId = await _userService.GetCurrentUserId();
            image.CreateDateTime = DateTime.Now;
            book.ImageId = image.Id;
            book.Image = image;

            _imageService.Delete(viewModel.ImagePath);
            var oldImage = await _unitOfWork.GetRepository<Image>().GetAsync(p => p.Id == oldImageId);
            await _unitOfWork.GetRepository<Image>().DeleteAsync(oldImage);
        }

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

        var image = await _unitOfWork.GetRepository<Image>().GetAsync(p => p.Id == book.ImageId);
        image.DeletedId = await _userService.GetCurrentUserId();
        image.DeleteDateTime = DateTime.Now;
        image.IsDeleted = true;

        await _unitOfWork.SaveAsync();
        return true;
    }

    public async Task<IEnumerable<IndexBookViewModel>> GetAllBooksNonDeletedAsync()
    {
        var books = await _unitOfWork.GetRepository<Book>()
            .GetAllAsync(p => !p.IsDeleted, c => c.Category, a => a.Author, p => p.Publisher, i => i.Image);
        var mapped = _mapper.Map<List<IndexBookViewModel>>(books);
        return mapped;
    }

    public async Task<IEnumerable<IndexBookViewModel>> GetBorrowableBooksAsync()
    {
        var userId = await _userService.GetCurrentUserId();

        var books = await _unitOfWork.GetRepository<Book>()
            .GetAllAsync(p => !p.IsDeleted, c => c.Category, a => a.Author, p => p.Publisher, i => i.Image);

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
        var book = await _unitOfWork.GetRepository<Book>().GetAsync(p => p.Id == id, i => i.Image);
        var mapped = _mapper.Map<UpdateBookViewModel>(book);
        return mapped;
    }

    public async Task<DetailBookViewModel?> GetBookByIdWithDetailBookViewModelAsync(string id)
    {
        var book = await _unitOfWork.GetRepository<Book>()
            .GetAsync(p => p.Id == id, a => a.Author, p => p.Publisher, c => c.Category, i => i.Image);
        var mapped = _mapper.Map<DetailBookViewModel>(book);
        return mapped;
    }

    public async Task<Dictionary<string, int>> CountBooksToStatusNonDeletedAsync()
    {
        var waitingToApprove = await _unitOfWork.GetRepository<Borrow>()
            .CountAsync(p => !p.IsDeleted && p.IsReturned && !p.IsApproved);
        var waitingToReturn = await _unitOfWork.GetRepository<Borrow>().CountAsync(p => !p.IsDeleted && !p.IsReturned);
        var inLibrary = await _unitOfWork.GetRepository<Book>().SumAsync(s => s.Amount, p => !p.IsDeleted);
        return new Dictionary<string, int>()
        {
            { "WaitingToApprove", waitingToApprove },
            { "WaitingToReturn", waitingToReturn },
            { "InLibrary", Convert.ToInt32(inLibrary) }
        };
    }

    public async Task<(int NonDeleted, int Deleted)> CountBooksAsync()
    {
        var nonDeleted = await _unitOfWork.GetRepository<Book>().CountAsync(p => !p.IsDeleted);
        var deleted = await _unitOfWork.GetRepository<Book>().CountAsync(p => p.IsDeleted);
        return (nonDeleted, deleted);
    }
}