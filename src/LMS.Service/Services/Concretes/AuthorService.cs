using AutoMapper;
using LMS.Data.UnitOfWorks;
using LMS.Entity.Entities;
using LMS.Entity.ViewModels.Author;
using LMS.Service.Services.Abstracts;

namespace LMS.Service.Services.Concretes;

public class AuthorService : IAuthorService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;
    private readonly IBookService _bookService;

    public AuthorService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService, IBookService bookService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userService = userService;
        _bookService = bookService;
    }

    public async Task<bool> CreateNewAuthorAsync(CreateAuthorViewModel viewModel)
    {
        if (await _unitOfWork.GetRepository<Author>().AnyAsync(p => p.FullName == viewModel.FullName)) return false;
        var mapped = _mapper.Map<Author>(viewModel);
        mapped.CreatedId = await _userService.GetCurrentUserId();
        await _unitOfWork.GetRepository<Author>().AddAsync(mapped);
        await _unitOfWork.SaveAsync();
        return true;
    }

    public async Task<bool> UpdateAuthorAsync(UpdateAuthorViewModel viewModel)
    {
        if (await _unitOfWork.GetRepository<Author>()
                .AnyAsync(p => p.FullName == viewModel.FullName && p.Id != viewModel.Id))
            return false;

        var flag = await _unitOfWork.GetRepository<Author>().AnyAsync(p => p.Id == viewModel.Id);

        if (!flag) return false;

        var author = await _unitOfWork.GetRepository<Author>().GetAsync(p => p.Id == viewModel.Id);
        _mapper.Map(viewModel, author);
        author.UpdatedId = await _userService.GetCurrentUserId();
        author.UpdateDateTime = DateTime.Now;
        await _unitOfWork.SaveAsync();
        return true;
    }

    public async Task<bool> DeleteAuthorWithIdAsync(string id)
    {
        var flag = await _unitOfWork.GetRepository<Author>().AnyAsync(p => p.Id == id);

        if (!flag) return false;

        var author = await _unitOfWork.GetRepository<Author>().GetAsync(p => p.Id == id);
        author.DeletedId = await _userService.GetCurrentUserId();
        author.DeleteDateTime = DateTime.Now;
        author.IsDeleted = true;

        var books = await _unitOfWork.GetRepository<Book>().GetAllAsync(p => p.AuthorId == author.Id);
        foreach (var book in books) await _bookService.DeleteBookWithIdAsync(book.Id);

        await _unitOfWork.SaveAsync();
        return true;
    }

    public async Task<IEnumerable<IndexAuthorViewModel>> GetAllAuthorsNonDeletedAsync()
    {
        var authors = await _unitOfWork.GetRepository<Author>().GetAllAsync(p => !p.IsDeleted, i => i.Books);
        var mapped = _mapper.Map<List<IndexAuthorViewModel>>(authors);
        return mapped;
    }

    public async Task<UpdateAuthorViewModel?> GetAuthorByIdWithUpdateViewModelAsync(string id)
    {
        var author = await _unitOfWork.GetRepository<Author>().GetAsync(p => p.Id == id);
        var mapped = _mapper.Map<UpdateAuthorViewModel>(author);
        return mapped;
    }

    public async Task<Dictionary<string, string>> GetAuthorsWithKeyAndNameAsync()
    {
        var authors = await _unitOfWork.GetRepository<Author>().GetAllAsync(p => !p.IsDeleted);
        return authors.ToDictionary(k => k.Id, v => v.FullName);
    }

    public async Task<(int NonDeleted, int Deleted)> CountAuthorsAsync()
    {
        var nonDeleted = await _unitOfWork.GetRepository<Author>().CountAsync(p => !p.IsDeleted);
        var deleted = await _unitOfWork.GetRepository<Author>().CountAsync(p => p.IsDeleted);
        return (nonDeleted, deleted);
    }
}